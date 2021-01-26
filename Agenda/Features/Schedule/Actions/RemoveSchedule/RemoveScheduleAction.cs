using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Features.Schedule
{
    public partial class ScheduleState
    {
        public class RemoveScheduleAction : IAction
        {
            public int SpeechId { get; private set; }

            public RemoveScheduleAction(int speechId)
            {
                SpeechId = speechId;
            }
        }
    }
}
