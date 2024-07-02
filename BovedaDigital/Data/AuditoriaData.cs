using SmcApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace SmcApi.Data
{
    public class AuditoriaData
    {

        /* public static bool Registrar(Auditoria oAuditoria)
         {

             using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
             {
                 SqlCommand cmd = new SqlCommand("aud_registrar", oConexion);
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters.AddWithValue("@idArchivo", oAuditoria.IdArchivo);
                 cmd.Parameters.AddWithValue("@tipoAccion", oAuditoria.TipoAccion);
                 cmd.Parameters.AddWithValue("@tipoUsuario", oAuditoria.TipoUsuario);
                 cmd.Parameters.AddWithValue("@tipoBusqueda", oAuditoria.TipoAccion == "Busqueda" ? oAuditoria.TipoBusqueda : DBNull.Value);

                 try
                 {
                     oConexion.Open();
                     cmd.ExecuteNonQuery();
                     return true;
                 }
                 catch (Exception ex)
                 {
                     return false;
                 }
             }
         }

         public static List<Auditoria> Listar()
         {
             List<Auditoria> oUsuario = new List<Auditoria>();
             using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
             {
                 SqlCommand cmd = new SqlCommand("aud_listar", oConexion);
                 cmd.CommandType = CommandType.StoredProcedure;

                 try
                 {
                     oConexion.Open();
                     cmd.ExecuteNonQuery();

                     using (SqlDataReader dr = cmd.ExecuteReader())
                     {

                         while (dr.Read())
                         {
                             oUsuario.Add(new Auditoria()
                             {
                                 Id = Convert.ToInt32(dr["Id"]),
                                 IdArchivo = Convert.ToInt32(dr["IdArchivo"]),
                                 FechaHora = (DateTime)dr["FechaHora"],
                                 TipoAccion = dr["TipoAccion"].ToString(),
                                 TipoUsuario = dr["TipoUsuario"].ToString(),
                                 TipoBusqueda = dr["TipoBusqueda"].ToString()
                             });
                         }

                     }



                     return oUsuario;
                 }
                 catch (Exception ex)
                 {
                     return oUsuario;
                 }
             }
         }

         public static List<Auditoria> ObtenerUnico(int id)
         {
             List<Auditoria> auditoria = new List<Auditoria>();
             using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
             {
                 string query = "SELECT * FROM Auditoria WHERE IdArchivo = @idArchivo";
                 SqlCommand cmd = new SqlCommand(query, oConexion);
                 cmd.Parameters.AddWithValue("@idArchivo", id);

                 try
                 {
                     oConexion.Open();
                     using (SqlDataReader dr = cmd.ExecuteReader())
                     {
                         while (dr.Read())
                         {
                             auditoria.Add(new Auditoria()
                             {
                                 Id = Convert.ToInt32(dr["Id"]),
                                 TipoAccion = dr["TipoAccion"].ToString(),
                                 TipoUsuario = dr["TipoUsuario"].ToString(),
                                 TipoBusqueda = dr["TipoBusqueda"].ToString(),
                                 FechaHora = (DateTime)dr["FechaHora"]
                             });
                         }
                     }

                     return auditoria;
                 }
                 catch (Exception ex)
                 {
                     // Manejo de excepciones
                     return null;
                 }
             }

         }*/
    }
}
