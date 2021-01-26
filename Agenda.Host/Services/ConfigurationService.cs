using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Host.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetClientId()
        {
            return _configuration.GetValue<string>("ClientId");
        }

        public string GetSecretId()
        {
            return _configuration.GetValue<string>("SecretId");
        }

        public string GetLinkedinUrl()
        {
            return _configuration.GetValue<string>("LinkedinUrl");
        }

        public string GetLinkedinApi()
        {
            return _configuration.GetValue<string>("LinkedinApi");
        }

        public string GetJwtSecret()
        {
            return _configuration.GetValue<string>("JwtSecret");
        }
    }
}
