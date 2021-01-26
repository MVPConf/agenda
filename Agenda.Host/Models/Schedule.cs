using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Host.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Speech Speech { get; set; }
    }
}
