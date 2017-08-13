using System.Configuration;

namespace Demo
{
    public class DbHelper
    {
        public static string ConnectionString =
            ConfigurationManager.ConnectionStrings["Demo.Properties.Settings.EmployeeDBConnectionString"].ToString();
    }
}