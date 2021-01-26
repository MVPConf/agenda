using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class SiteSpeech
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Track { get; set; }
        public bool Visible { get; set; }
        public string Scheduler { get; set; }
        public List<string> Speakers { get; set; }
        public string JoinUrl { get; set; }
        public string LinkUrl { get; set; }
    }
}
