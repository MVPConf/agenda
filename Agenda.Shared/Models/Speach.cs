using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class Speech
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Track { get; set; }
        public bool Visible { get; set; }
        public string Schedule { get; set; }
        public List<string> Speakers { get; set; } = new List<string>();
        public string JoinUrl { get; set; }
        public string LinkUrl { get; set; }
        public int Scheduled { get; set; }
    }
}
