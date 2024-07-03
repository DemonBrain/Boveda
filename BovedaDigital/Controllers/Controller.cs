using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmcApi.Data;
using SmcApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text;
using System;
using static Dropbox.Api.Files.ListRevisionsMode;
using static Google.Apis.Storage.v1.Data.Bucket.LifecycleData;

namespace SmcApi.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class Controller : ControllerBase
    {
        /*------------------------  IMPORTANTES  ------------------------*/



        [HttpPost("/SubirArchivo")]
        public async Task<Archivos> PostArchivo([FromForm] ArchivoUpload model, int codComuna)
        {
            Archivos archivo = new Archivos();
            Versiones version = new Versiones();
            var ipAddress = HttpContext.Connection.RemoteIpAddress;


            archivo.Nombre = System.IO.Path.GetFileNameWithoutExtension(model.FileUpload.FileName);
            archivo.Extension = Extension(model.FileUpload.FileName);
            archivo.Tema = model.Tema;
            archivo.Glosa = model.Glosa;
            archivo.RolesConsulta = model.RolesConsulta;
            archivo.RolesEscritura = model.RolesEscritura;
            archivo.comuna = codComuna;

            


            Accesos Acc = Pruebas.InfoAccesosXcomuna(codComuna);
            version.IdAccesos = Acc.Id;
            string url = string.Empty;
            
            switch (Acc.Nube)
            {
                case "Amazon":
                    url = await AmazonData.UploadFileAsync(model.FileUpload, Acc.Bucket, Acc.Credenciales, 0);
                    break;

                case "Cloude":
                    url = await CloudData.UploadFile(model.FileUpload, Acc.Bucket, Acc.Credenciales, 0);
                    break;

                case "Azure":
                    url = await AzureData.UploadAsync(model.FileUpload, Acc.Bucket, Acc.Credenciales, 0);
                    break;

                case "DropBox":
                    url = await DropboxData.UploadAsync(model.FileUpload, Acc.Bucket, Acc.Credenciales, 0);
                    break;

                default:
                    break;
            }

            version.URLArchivo = url;

            archivo.Id = Pruebas.Archivo(archivo);

            version.IdArchivo = archivo.Id;

            DateTime now = DateTime.Now;
            version.Id = Pruebas.Version(model.Identificador+":"+ipAddress, (int)archivo.Id, version);
            version.NumeroVersion = Pruebas.MaxVersion((int)archivo.Id);
            version.FechaRegistro = now;

            archivo.Versiones.Add(version);


            return archivo;
        }

        [HttpPost("/SubirVersion")]
        public async Task<Archivos> PostVersion([FromForm] VersionUpload model, int idArchivo, int codComuna, string rol)
        {
            Versiones version = new Versiones();
            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            Accesos Acc = Pruebas.InfoAccesosXcomuna(codComuna);

            int VersionMax = Pruebas.MaxVersion(idArchivo) + 1;

            if (!Pruebas.PermisoRol(idArchivo, rol))
            {
                return null;
            }

            string url = string.Empty;

            switch (Acc.Nube)
            {
                case "Amazon":
                    url = await AmazonData.UploadFileAsync(model.FileUpload, Acc.Bucket, Acc.Credenciales, VersionMax);
                    break;

                case "Cloude":
                    url = await CloudData.UploadFile(model.FileUpload, Acc.Bucket, Acc.Credenciales, VersionMax);
                    break;

                case "Azure":
                    url = await AzureData.UploadAsync(model.FileUpload, Acc.Bucket, Acc.Credenciales, VersionMax);
                    break;

                case "DropBox":
                    url = await DropboxData.UploadAsync(model.FileUpload, Acc.Bucket, Acc.Credenciales, VersionMax);
                    break;

                default:
                    break;
            }

            version.URLArchivo = url;
            version.IdAccesos = Acc.Id;

            int idVersion = Pruebas.VersionModificar(rol, model.Identificador + ":" + ipAddress, idArchivo, version);

            Archivos archivos = Pruebas.BuscarArchivoYVersion(idVersion);

            return archivos;
        }

        [HttpPost("/BusquedaArchivo")]
        public List<Archivos> BusquedaArchivo(string columna, string valor, string rol, string identificador)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            return Pruebas.BuscarArchivos(columna,identificador + ":" + ipAddress, valor, rol);
        }

        [HttpGet("/DescargarArchivo")]
        public async Task<ArchivoDownload> GetArchivo(int idArchivo, int? idVersion, string identificador)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress;

            if (!idVersion.HasValue)
            {
                idVersion = Pruebas.MaxIdVersion(idArchivo);
            }

            Archivos archivos = Pruebas.BuscarArchivoYVersion((int)idVersion);
            int id = (int)archivos.Id;
            Versiones version = archivos.Versiones[0];
            Accesos Acc = Pruebas.InfoAccesosXid((int)version.IdAccesos);


            Task<byte[]> descarga = Acc.Nube switch
            {
                "Amazon" => AmazonData.DownloadFileAsync(archivos.Nombre + "." + archivos.Extension, Acc.Bucket, Acc.Credenciales, (int)version.NumeroVersion),
                "Cloude" => CloudData.GetFile(archivos.Nombre + "." + archivos.Extension, Acc.Bucket, Acc.Credenciales, (int)version.NumeroVersion),
                "Azure" => AzureData.DownloadAsync(archivos.Nombre + "." + archivos.Extension, Acc.Bucket, Acc.Credenciales, (int)version.NumeroVersion),
                "DropBox" => DropboxData.DownloadAsync(archivos.Nombre + "." + archivos.Extension, Acc.Bucket, Acc.Credenciales, (int)version.NumeroVersion),
                _ => Task.FromResult<byte[]>(null)
            };


            byte[] fileData = await descarga;


            string base64String = Convert.ToBase64String(fileData);

            Auditoria auditoria = new Auditoria()
            {
                IdVersion = idVersion,
                TipoAccion = 4,
                CriterioBusqueda = "",
                Identificador = identificador + ":" + ipAddress
            };

            Pruebas.AuditoriaDescarga(auditoria);




            ArchivoDownload ultimo = new ArchivoDownload();

            ultimo.IdVersion = (int)idVersion;

            ultimo.Binario = base64String;

            ultimo.Archivos = Pruebas.ListarElArchivo(id);

            ultimo.Audi = Pruebas.ListarAuditoria(id);



            return ultimo;
        }









        /*------------------------  APOYO  ------------------------*/




        [HttpPost("/RegistroMunicipalidad")]

        public async Task<string> NuevoMunicipalidad(string rol, [FromForm] Municipalidades munic)
        {
            string muni = Pruebas.Municipalidad(rol, munic);

            return muni;
        }

        [HttpPost("/RegistroAcceso")]

        public async Task<string> NuevoAcceso(string rol, [FromForm] Accesos acce)
        {
            string acc = Pruebas.Accesos(rol,acce);

            return acc;
        }

        [HttpPost("/NuevaCredencial")]

        public async Task<string> NuevaCredencial(int acceso, string credencial)
        {
            return Pruebas.NuevaCredencial(acceso, credencial);
        }

        [HttpGet("/ListaMunicipalidades")]

        public List<Municipalidades> ListarMunicipalidades()
        {
            return Pruebas.ListarMunicipalidades();
        }

        [HttpGet("/ListaAccesos")]

        public List<Accesos> ListarAccesos()
        {
            return Pruebas.ListarAccesos(); 
        }

        [HttpGet("/ListaAuditoria")]

        public List<Auditoria> ListarAuditoria(int idArchivo)
        {
            return Pruebas.ListarAuditoria(idArchivo);
        }

        [HttpPost("/ListarElArchivo")]

        public Archivos BusquedaUnica(int idArchivo)
        {
            return Pruebas.ListarElArchivo(idArchivo);
        }

        public static string Extension(string archivo)
        {
            string extension = System.IO.Path.GetExtension(archivo);
            return extension.Length > 0 ? extension.Substring(1) : "";
        }

    }














    public class ArchivoUpload
    {
        public IFormFile FileUpload { get; set; }
        public string Identificador { get; set; }
        public string Tema {  get; set; }
        public string Glosa { get; set; }
        public string RolesConsulta { get; set; }
        public string RolesEscritura { get; set; }
    }

    public class VersionUpload
    {
        public IFormFile FileUpload { get; set; }
        public string Identificador { get; set; }
    }

    public class ArchivoDownload
    {
        public int IdVersion {get; set;}
        public string Binario { get; set; }
        public Archivos Archivos { get; set; }
        public List<Auditoria> Audi { get; set; }
    }

    }















