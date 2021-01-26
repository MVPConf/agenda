using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Host.Models
{
    public class Speech
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Track { get; set; }
        public bool Visible { get; set; }
        public DateTime? Schedule { get; set; }
        public List<string> Speakers { get; set; } = new List<string>();
        public string JoinUrl { get; set; }
        public string LinkUrl { get; set; }

        public virtual List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
