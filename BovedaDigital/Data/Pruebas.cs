using SmcApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using static Dropbox.Api.Files.ListRevisionsMode;
using static Dropbox.Api.TeamLog.SharedLinkAccessLevel;

namespace SmcApi.Data
{
    public class Pruebas
    {
        public static int Archivo(Archivos oArchivo)
        {

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("archivos_create", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", oArchivo.Nombre);
                cmd.Parameters.AddWithValue("@extension", oArchivo.Extension);
                cmd.Parameters.AddWithValue("@tema", oArchivo.Tema);
                cmd.Parameters.AddWithValue("@glosa", oArchivo.Glosa);
                cmd.Parameters.AddWithValue("@rolesConsulta", oArchivo.RolesConsulta);
                cmd.Parameters.AddWithValue("@rolesEscritura", oArchivo.RolesEscritura);
                cmd.Parameters.AddWithValue("@versionActual", 0);
                cmd.Parameters.AddWithValue("@comuna", oArchivo.comuna);


                // Parámetro de salida para capturar el ID del último registro insertado

                SqlParameter outputIdParam = new SqlParameter("@ID", SqlDbType.Int);
                outputIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputIdParam);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    int IdArchivo = Convert.ToInt32(cmd.Parameters["@ID"].Value);
                    
                    return IdArchivo; // O cualquier otro manejo que necesites hacer con el ID
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    Console.WriteLine("Error al insertar el registro: " + ex.Message);
                    return 0; // Retornar algún valor de error
                }
            }
        }

        public static int VersionModificar(string rol, string identificador, int idArchivo, Versiones oVersion)
        {

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("versiones_createModificar", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                cmd.Parameters.AddWithValue("@idAccesos", oVersion.IdAccesos);
                cmd.Parameters.AddWithValue("@urlArchivo", oVersion.URLArchivo);
                cmd.Parameters.AddWithValue("@identificador", identificador);
                cmd.Parameters.AddWithValue("@rol",rol);

                // Parámetro de salida para capturar el ID del último registro insertado

                SqlParameter outputIdParam = new SqlParameter("@ID", SqlDbType.Int);
                outputIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputIdParam);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    int IdFile = Convert.ToInt32(cmd.Parameters["@ID"].Value);

                    return IdFile; // O cualquier otro manejo que necesites hacer con el ID
                }
                catch (Exception ex)
                {
                    
                    return 0; // Retornar algún valor de error
                }
            }
        }

        public static int Version(string identificador, int idArchivo, Versiones oVersion)
        {

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("versiones_create", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idArchivo", idArchivo);
                cmd.Parameters.AddWithValue("@idAccesos", oVersion.IdAccesos);
                cmd.Parameters.AddWithValue("@urlArchivo", oVersion.URLArchivo);
                cmd.Parameters.AddWithValue("@identificador", identificador);

                // Parámetro de salida para capturar el ID del último registro insertado

                SqlParameter outputIdParam = new SqlParameter("@ID", SqlDbType.Int);
                outputIdParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputIdParam);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    int IdFile = Convert.ToInt32(cmd.Parameters["@ID"].Value);

                    return IdFile; // O cualquier otro manejo que necesites hacer con el ID
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    Console.WriteLine("Error al insertar el registro: " + ex.Message);
                    return 0; // Retornar algún valor de error
                }
            }
        }

        public static string Municipalidad(string rol, Municipalidades oMuni)
        {

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("municipalidades_create", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoComuna", oMuni.CodigoComuna);
                cmd.Parameters.AddWithValue("@comuna", oMuni.Comuna);
                cmd.Parameters.AddWithValue("@rol", rol);


                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();


                    return "si"; 
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    Console.WriteLine("Error al insertar el registro: " + ex.Message);
                    return ex.Message; // Retornar algún valor de error
                }
            }
        }

        public static string Accesos(string rol, Accesos oAcc)
        {

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("accesos_create", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoComuna", oAcc.CodigoComuna);
                cmd.Parameters.AddWithValue("@nube", oAcc.Nube);
                cmd.Parameters.AddWithValue("@bucket", oAcc.Bucket);
                cmd.Parameters.AddWithValue("@credenciales", oAcc.Credenciales);
                cmd.Parameters.AddWithValue("@rol", rol);
                /* Cada vez que se crea hay que tambien crear el vigencia_create*/


                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();


                    return "si";
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    Console.WriteLine("Error al insertar el registro: " + ex.Message);
                    return ex.Message; // Retornar algún valor de error
                }
            }
        }

        public static List<Municipalidades> ListarMunicipalidades()
        {
            List<Municipalidades> listaMunicipalidades = new List<Municipalidades>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {

                SqlCommand cmd = new SqlCommand("municipalidades_listar", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                    {
                        oConexion.Open();
                        cmd.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Municipalidades municipalidad = new Municipalidades
                                {
                                    CodigoComuna = Convert.ToInt32(dr["CODIGOCOMUNA"]),
                                    Comuna = dr["COMUNA"].ToString()
                                };
                                listaMunicipalidades.Add(municipalidad);
                            }
                        }
                    return listaMunicipalidades;
                }
                    catch (Exception ex)
                    {
                    return listaMunicipalidades;
                }
                
            }

           
        }

        public static List<Accesos> ListarAccesos()
        {
            List<Accesos> listaAccesos = new List<Accesos>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {

                SqlCommand cmd = new SqlCommand("accesos_listar", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        oConexion.Open();
                        cmd.ExecuteNonQuery();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Accesos acceso = new Accesos
                                {
                                    Id = Convert.ToInt32(dr["IDACCESO"]),
                                    CodigoComuna = Convert.ToInt32(dr["CODIGOCOMUNA"]),
                                    Nube = dr["NUBE"].ToString(),
                                    Bucket = dr["BUCKET"].ToString(),
                                    Credenciales = dr["CREDENCIALES"].ToString()
                                };
                                listaAccesos.Add(acceso);
                            }
                        }
                    return listaAccesos;
                }
                    catch (Exception ex)
                    {
                    // Manejar la excepción aquí
                    return listaAccesos;
                }
                
            }

            
        }

        public static List<Auditoria> ListarAuditoria(int idArchivo)
        {
            List<Auditoria> listaAudi = new List<Auditoria>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {

                SqlCommand cmd = new SqlCommand("auditoria_listar_idArchivo", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDARCHIVO", idArchivo);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Auditoria audi = new Auditoria
                            {
                                Id = Convert.ToInt32(dr["IDAUDITORIA"]),
                                IdVersion = Convert.ToInt32(dr["IDVERSION"]),
                                TipoAccion = Convert.ToInt32(dr["TIPOACCION"]),
                                CriterioBusqueda = dr["CRITERIOBUSQUEDA"].ToString(),
                                FechaHora = Convert.ToDateTime(dr["FECHAACCION"]),
                                Identificador = dr["IDENTIFICADOR"].ToString()
                            };
                            listaAudi.Add(audi);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                }

            }

            return listaAudi;
        }

        public static List<Archivos> BuscarArchivos(string columna, string identificador, string valor, string rol)
        {
            List<Archivos> oArchivos = new List<Archivos>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("archivos_buscar", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@columna", columna);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@identificador", identificador);
                cmd.Parameters.AddWithValue("@rol", rol);

                try
                {
                    oConexion.Open();
                   

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (dr.Read())
                            {
                                Archivos archivo = new Archivos()
                                {
                                    Id = Convert.ToInt32(dr["IDARCHIVO"]),
                                    Nombre = dr["NOMBRE"].ToString(),
                                    Extension = dr["EXTENSION"].ToString(),
                                    Tema = dr["TEMA"].ToString(),
                                    Glosa = dr["GLOSA"].ToString(),
                                    RolesConsulta = dr["ROLESCONSULTA"].ToString(),
                                    RolesEscritura = dr["ROLESESCRITURA"].ToString(),
                                    Versiones = new List<Versiones>()
                                };

                                archivo.Versiones.Add(new Versiones()
                                {
                                    Id = Convert.ToInt32(dr["IDVERSION"]),
                                    IdArchivo = Convert.ToInt32(dr["IDARCHIVO"]),
                                    IdAccesos = Convert.ToInt32(dr["IDACCESO"]),
                                    URLArchivo = dr["URLARCHIVO"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(dr["FECHAREGISTRO"]),
                                    NumeroVersion = Convert.ToInt32(dr["NUMEROVERSION"])
                                });

                                oArchivos.Add(archivo);
                            }
                        } while (dr.NextResult());


                    } 


                    return oArchivos;
                }
                catch (Exception ex)
                {
                    return oArchivos;
                }
            }
        }

        public static Archivos ListarElArchivo(int idArchivo)
        {
            Archivos listaArchivos = new Archivos();

            using (SqlConnection conexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("archivos_listar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdArchivo", idArchivo);

                try
                {
                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Archivos archivo = new Archivos
                            {
                                Id = Convert.ToInt32(dr["IDARCHIVO"]),
                                Nombre = dr["NOMBRE"].ToString(),
                                Extension = dr["EXTENSION"].ToString(),
                                Tema = dr["TEMA"].ToString(),
                                Glosa = dr["GLOSA"].ToString(),
                                RolesConsulta = dr["ROLESCONSULTA"].ToString(),
                                RolesEscritura = dr["ROLESESCRITURA"].ToString(),
                                Versiones = new List<Versiones>()
                            };
                            listaArchivos= archivo;
                        }
                    }

                    
                    
                        cmd = new SqlCommand("versiones_listar", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdArchivo", idArchivo);
                        cmd.ExecuteNonQuery();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Versiones version = new Versiones
                                {
                                    Id = Convert.ToInt32(dr["IDVERSION"]),
                                    IdArchivo = Convert.ToInt32(dr["IDARCHIVO"]),
                                    IdAccesos = Convert.ToInt32(dr["IDACCESO"]),
                                    URLArchivo = dr["URLARCHIVO"].ToString(),
                                    FechaRegistro = Convert.ToDateTime(dr["FECHAREGISTRO"]),
                                    NumeroVersion = Convert.ToInt32(dr["NUMEROVERSION"])
                                };
                              listaArchivos.Versiones.Add(version);
                            }
                        }
                    

                    return listaArchivos;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    return listaArchivos;
                }
            }
        }

        public static string NuevaCredencial(int idAcceso, string credenciales)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("Credenciales_create", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDACCESO", idAcceso);
                cmd.Parameters.AddWithValue("@CREDENCIALES", credenciales);


                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();


                    return "si";
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    Console.WriteLine("Error al insertar el registro: " + ex.Message);
                    return ex.Message; // Retornar algún valor de error
                }
            }
        }

        /* --- APOYO --- */

        public static Accesos InfoAccesosXcomuna(int cod)
        {
            Accesos Acc = new Accesos();

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {

                SqlCommand cmd = new SqlCommand("acceso_listar_x_codigo", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CODIGOCOMUNA", cod);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Accesos acceso = new Accesos
                            {
                                Id = Convert.ToInt32(dr["IDACCESO"]),
                                CodigoComuna = Convert.ToInt32(dr["CODIGOCOMUNA"]),
                                Nube = dr["NUBE"].ToString(),
                                Bucket = dr["BUCKET"].ToString(),
                                Credenciales = dr["CREDENCIALES"].ToString()
                            };
                            Acc = acceso;
                        }
                    }
                    return Acc;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    return Acc;
                }

            }
        }

        public static Accesos InfoAccesosXid(int id)
        {
            Accesos Acc = new Accesos();

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {

                SqlCommand cmd = new SqlCommand("acceso_listar_x_id", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDACCESO", id);

                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Accesos acceso = new Accesos
                            {
                                Id = Convert.ToInt32(dr["IDACCESO"]),
                                CodigoComuna = Convert.ToInt32(dr["CODIGOCOMUNA"]),
                                Nube = dr["NUBE"].ToString(),
                                Bucket = dr["BUCKET"].ToString(),
                                Credenciales = dr["CREDENCIALES"].ToString()
                            };
                            Acc = acceso;
                        }
                    }
                    return Acc;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    return Acc;
                }

            }
        }

        public static int MaxVersion(int idArchivo)
        {
            int valor;

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {

                SqlCommand cmd = new SqlCommand("ObtenerVersionMaxima", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDARCHIVO", idArchivo);

                try
                {
                    oConexion.Open();
                    var result = cmd.ExecuteScalar();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        valor = Convert.ToInt32(result);
                    }
                    return valor;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    return -1;
                }

            }
        }

        public static int MaxIdVersion(int idArchivo)
        {
            

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                int valor = 0;
                SqlCommand cmd = new SqlCommand("ObtenerVersionActual", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDARCHIVO", idArchivo);

                try
                {
                    oConexion.Open();
                    var result = cmd.ExecuteScalar();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        valor = Convert.ToInt32(result);
                    }
                    return valor;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    return -1;
                }

            }
        }

        public static bool PermisoRol(int idArchivo, string rol)
        {
            bool rolPresente = false;

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("VerificarRol", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDARCHIVO", idArchivo);
                cmd.Parameters.AddWithValue("@ROL", rol);

                try
                {
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string mensaje = dr["Mensaje"].ToString();
                            rolPresente = mensaje.Contains("El rol está presente");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    Console.WriteLine($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
                }

                return rolPresente;
            }
        }

        public static Archivos BuscarArchivoYVersion(int idVersion)
        {
            Archivos oArchivos = new Archivos();

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("ObtenerInformacionArchivoYVersion", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdVersion", idVersion);

                try
                {
                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Archivos archivo = new Archivos
                            {
                                Id = Convert.ToInt32(dr["IdArchivo"]),
                                Nombre = dr["NombreArchivo"].ToString(),
                                Extension = dr["ExtensionArchivo"].ToString(),
                                Tema = dr["TemaArchivo"].ToString(),
                                Glosa = dr["GLOSA"].ToString(),
                                Versiones = new List<Versiones>()
                            };

                            Versiones version = new Versiones
                            {
                                Id = Convert.ToInt32(dr["NumeroVersion"]),
                                IdArchivo = Convert.ToInt32(dr["IdArchivo"]),
                                IdAccesos = Convert.ToInt32(dr["IdAcceso"]),
                                URLArchivo = dr["UrlArchivo"].ToString(),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                                NumeroVersion = Convert.ToInt32(dr["NumeroVersion"])
                            };

                            archivo.Versiones.Add(version);
                            oArchivos = archivo;
                        }
                    }

                    return oArchivos;
                }
                catch (Exception ex)
                {
                    // Manejar la excepción aquí
                    Console.WriteLine($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
                    return oArchivos;
                }
            }
        }

        public static bool AuditoriaDescarga(Auditoria auditoria)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("InsertAuditoriaVersion", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDVERSION", auditoria.IdVersion);
                cmd.Parameters.AddWithValue("@TIPOACCION", auditoria.TipoAccion);
                cmd.Parameters.AddWithValue("@CRITERIOBUSQUEDA", auditoria.CriterioBusqueda);
                cmd.Parameters.AddWithValue("@IDENTIFICADOR", auditoria.Identificador);
                


                try
                {
                    oConexion.Open();
                    cmd.ExecuteNonQuery();


                    return true;
                }
                catch (Exception ex)
                {
                    // Manejar excepciones
                    Console.WriteLine("Error al insertar el registro: " + ex.Message);
                    return false; // Retornar algún valor de error
                }
            }
        }

    }
}
