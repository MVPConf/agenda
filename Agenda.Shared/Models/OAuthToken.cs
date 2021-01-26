using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class OAuthToken
    {
        public OAuthToken()
        {

        }

        public OAuthToken(string accessToken, long expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }
    }
}
