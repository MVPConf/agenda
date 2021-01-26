using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class AuthRequest
    {
        public string Code { get; set; }

        public string RedirectUri { get; set; }

        public AuthRequest(string code, string redirectUri)
        {
            Code = code;
            RedirectUri = redirectUri;
        }
    }
}
