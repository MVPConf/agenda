using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class CreateSchedule
    {
        public CreateSchedule()
        {

        }

        public CreateSchedule(int speechId)
        {
            SpeechId = speechId;
        }

        public int SpeechId { get; set; }
    }
}
