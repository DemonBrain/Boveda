using System.Data.SqlClient;

namespace SmcApi.Data
{
    public class UsuariosData
    {
        public static bool Verificacion(string user, string pass)
        {
            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT Password FROM Usuarios WHERE Cuenta = @cuenta", oConexion);
                cmd.Parameters.AddWithValue("@cuenta", user);

                try
                {
                    oConexion.Open();
                    string contraseñaAlmacenada = (string)cmd.ExecuteScalar();
                    if (contraseñaAlmacenada != null && contraseñaAlmacenada.Equals(pass))
                    {
                        return true; // La contraseña es correcta
                    }
                   else
                    {
                        return false; // La contraseña no coincide
                    }
                }
                catch (Exception ex)
                {
                   
                    Console.WriteLine("Error al verificar la contraseña: " + ex.Message);
                    return false;
                }
            }
        }

        public static List<string> ObtenerRoles()
        {
            List<string> roles = new List<string>();

            using (SqlConnection oConexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT Rol FROM Usuarios", oConexion);

                try
                {
                    oConexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        roles.Add(reader["Rol"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                }
            }

            return roles;
        }

        public static string ObtenerRol(string usuario, string contraseña)
        {
            string rol = null;

            using (SqlConnection conexion = new SqlConnection(Conexion.rutaConexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT Rol FROM Usuarios WHERE Cuenta = @Usuario AND Password = @Contraseña", conexion);
                cmd.Parameters.AddWithValue("@Usuario", usuario);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);

                try
                {
                    conexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        rol = reader["Rol"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    
                }
            }

            return rol;
        }

    }
}
