using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class UserRaffle
    {
        public UserRaffle(string userHash, string fullName, string email, string company)
        {
            UserHash = userHash;
            FullName = fullName;
            Email = email;
            Company = company;
        }

        public string UserHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
    }
}
