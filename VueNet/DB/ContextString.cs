using System;
using MySql.Data.MySqlClient;

namespace VueNet.DB
{
    public class DBContext
    {
        public static MySqlConnection MySqlConnection()
        {
            var option = DBConfigureServices.Configuration.GetSection("DbSettings")
                                                     .Get<DbSettings>();
            return new MySqlConnection(option.DBContext);
        }
    }

    public class DBConfigureServices
    {
        private static IConfiguration _configuration;
        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var environmentName = string.IsNullOrEmpty(environment) ? "Production" : environment;
                    var builder = new ConfigurationBuilder()
                                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                    .AddJsonFile(path: $"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
                    _configuration = builder.Build();
                }

                return _configuration;
            }
        }
    }
}

