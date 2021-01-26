using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class Slot
    {
        public Slot()
        {

        }

        public Slot(DateTime time)
        {
            Time = time;
        }

        public Slot(DateTime time, Speech speech)
        {
            Time = time;
            Speech = speech;
        }

        public DateTime Time { get; set; }
        public Speech Speech { get; set; }
    }
}
