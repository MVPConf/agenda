using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Host.Models
{
    public class Winner
    {
        public int Id { get; set; }
        public string UserHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
