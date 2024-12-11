using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.NLtoSQL.Services
{
    // This class is used to read the configuration from the appsettings.json file
    public class AzureConfiguration
    {
        private IConfiguration _config = null;
        public AzureConfiguration()
        {

            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            AOAIEndpoint = _config["Azure:AOAIEndpoint"];
            AOAIKey = _config["Azure:AOAIKey"];
            AOAIDeploymentId = _config["Azure:AOAIDeploymentId"];
            SQLHostname = _config["Azure:SQLHostname"];
            SQLUsername = _config["Azure:SQLUsername"];
            SQLPassword = _config["Azure:SQLPassword"];
            SQLDatabase = _config["Azure:SQLDatabase"];
        }
        public string AOAIEndpoint { get; set; }
        public string AOAIKey { get; set; }
        public string AOAIDeploymentId { get; set; }

        public string SQLHostname { get; set; }

        public string SQLUsername { get; set; }
        public string SQLPassword { get; set; }
        public string SQLDatabase { get; set; }
    }
 }
