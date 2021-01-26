using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Features.Schedule
{
    public partial class ScheduleState
    {
        public class ScheduleSpeechAction : IAction
        {
            public int SpeechId { get; set; }

            public ScheduleSpeechAction(int speechId)
            {
                SpeechId = speechId;
            }
        }
    }
}
