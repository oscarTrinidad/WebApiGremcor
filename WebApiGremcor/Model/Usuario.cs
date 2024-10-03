using System.Data;
using System.Data.SqlClient;

namespace WebApiGremcor.Model
{
    public class Usuario
    {
        public int iIdUsuario { get; set; }
        public string? sNombreUsuario { get; set; }
        public string? sCorreo { get; set; }
        public string? sDni { get; set; }
        public string? sUsuario { get; set; }
        public string? sContrasenia { get; set; }
        public int iEstadoUsuario { get; set; }
        public string? sEstadoUsuario { get; set; }
        public int iEstado { get; set; }
        public string? sEstado { get; set; }
        public int iRol { get; set; }
        public string? sRol { get; set; }

        public async Task<ResponseModel> InicioSesion(Usuario pUsuario)
        {
            ResponseModel response = new ResponseModel();
            Usuario? usuario = new Usuario();
            usuario = null;
            var vStatus = true;
            var vMensage = "";
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd;
            try
            {
                using (cn = ConexionDB.BdConexionSql())
                {
                    cmd = new SqlCommand("SEGURIDAD.sp_InicioSesion", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@vUsuario", SqlDbType.VarChar).Value = pUsuario.sUsuario;
                    cmd.Parameters.Add("@vContrasenia", SqlDbType.VarChar).Value = pUsuario.sContrasenia;
                    await cn.OpenAsync();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        if (dataReader.GetInt32(0) < 0)
                        {
                            vStatus = false;
                            vMensage = dataReader.GetString(1);
                        }
                        else
                        {
                            usuario = new Usuario();
                            usuario.iIdUsuario = dataReader.GetInt32(1);
                            usuario.sNombreUsuario = dataReader.GetString(2);
                            usuario.sCorreo = dataReader.GetString(3);
                            usuario.sDni = dataReader.GetString(4);
                            usuario.sUsuario = dataReader.GetString(5);
                            usuario.sContrasenia = dataReader.GetString(6);
                            usuario.iRol = dataReader.GetInt32(7);
                            usuario.sRol = dataReader.GetString(8);
                        }
                    }
                }

                response.status = vStatus;
                response.data = usuario;
                if (vMensage != "")
                {
                    response.message = vMensage;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }

        public async Task<List<Usuario>> ListaUsuario(Usuario pUsuario)
        {
            List<Usuario> usuarios = new List<Usuario>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd;
            try
            {
                using (cn = ConexionDB.BdConexionSql())
                {
                    cmd = new SqlCommand("SEGURIDAD.sp_ListaUsuario", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@vUsuario", SqlDbType.VarChar).Value = pUsuario.sUsuario;
                    await cn.OpenAsync();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.iIdUsuario = dataReader.GetInt32(0);
                        usuario.sNombreUsuario = dataReader.GetString(1);
                        usuario.sCorreo = dataReader.GetString(2);
                        usuario.sDni = dataReader.GetString(3);
                        usuario.sUsuario = dataReader.GetString(4);
                        usuario.iRol = dataReader.GetInt32(5);
                        usuario.sRol = dataReader.GetString(6);
                        usuario.iEstadoUsuario = dataReader.GetInt32(7);
                        usuario.sEstadoUsuario = dataReader.GetString(8);
                        usuario.iEstado = dataReader.GetInt32(9);
                        usuarios.Add(usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                //usuarios = new List<Usuario>();
                throw ex;
            }
            finally
            {
                cn.Close();
            }
            return usuarios;
        }
        public async Task<ResponseModel> MantenimientoUsuario(Usuario pUsuario)
        {
            ResponseModel response = new ResponseModel();
            var vStatus = true;
            var vData = 0;
            var vMensage = "";
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd;
            try
            {
                using (cn = ConexionDB.BdConexionSql())
                {
                    cmd = new SqlCommand("SEGURIDAD.sp_MantenimientoUsuario", cn);
                    cmd.Parameters.Add("@iIdUsuario", SqlDbType.Int).Value = pUsuario.iIdUsuario;
                    cmd.Parameters.Add("@vNombreUsuario", SqlDbType.VarChar).Value = pUsuario.sNombreUsuario;
                    cmd.Parameters.Add("@vCorreo", SqlDbType.VarChar).Value = pUsuario.sCorreo;
                    cmd.Parameters.Add("@vDni", SqlDbType.VarChar).Value = pUsuario.sDni;
                    cmd.Parameters.Add("@iRol", SqlDbType.Int).Value = pUsuario.iRol;
                    cmd.Parameters.Add("@iEstadoUsuario", SqlDbType.Int).Value = pUsuario.iEstadoUsuario;
                    cmd.Parameters.Add("@iEstado", SqlDbType.Int).Value = pUsuario.iEstado;
                    cmd.Parameters.Add("@vUsuario", SqlDbType.VarChar).Value = pUsuario.sUsuario;
                    cmd.CommandType = CommandType.StoredProcedure;
                    await cn.OpenAsync();
                    SqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    while (dataReader.Read())
                    {
                        if (dataReader.GetInt32(0) < 0)
                        {
                            vStatus = false;
                            vMensage = dataReader.GetString(1);
                        }
                        else
                        {
                            vStatus = true;
                            vData = dataReader.GetInt32(0);
                        }
                    }
                }
                response.status = vStatus;
                response.data = vData;
                if (vMensage != "")
                {
                    response.message = vMensage;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            return response;
        }
    }
}

