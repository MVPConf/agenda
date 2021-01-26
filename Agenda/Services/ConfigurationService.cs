using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebAssemblyHostEnvironment _hostEnvironment;

        public ConfigurationService(IConfiguration configuration, IWebAssemblyHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public string GetBaseHostUrl()
        {
            return _hostEnvironment.BaseAddress;
        }

        public string GetRedirectUri()
        {
            return $"{GetBaseHostUrl()}login/linkedin/callback";
        }

        public string GetClientId()
        {
            return _configuration.GetValue<string>("ClientId");
        }

        public string GetLinkedinUrl()
        {
            return _configuration.GetValue<string>("LinkedinUrl");
        }

        public string GetLoginUrl()
        {
            var clientId = GetClientId();
            var linkedinUrl = GetLinkedinUrl();
            var redirectUri = GetRedirectUri();
            var scopes = "r_liteprofile%20r_emailaddress";
            
            return $"{linkedinUrl}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scopes}";
        }
    }
}
