using System.Data.SqlClient;

namespace WebApiGremcor.Model
{
    public class ConexionDB
    {
        public static IConfiguration? configuration;

        public static IConfigurationRoot GetConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
            return builder;
        }
        public static SqlConnection BdConexionSql()
        {
            SqlConnection cn = new SqlConnection(obtenerstringSql());

            return cn;
        }
        public static string obtenerstringSql(string sCadena = "DefaultConnection")
        {
            //var sConexion = configuration.GetConnectionString("ConnectionStrings:DefaultConnection");
            //var sConexionI = configuration.GetConnectionString("DefaultConnection");
            var sConexionII = GetConnection().GetSection("ConnectionStrings").GetSection(sCadena).Value;
            return sConexionII;
        }
    }
}
