using Agenda.Shared.Models;
using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Features.Schedule
{
    public partial class ScheduleState : State<ScheduleState>
    {
        public UserSchedule Schedule { get; private set; }
        public bool Loading { get; private set; }

        public override void Initialize()
        {
            Loading = false;
            Schedule = new UserSchedule();
        }

        private void StartLoading()
        {
            Loading = true;
        }

        private void FinishLoading()
        {
            Loading = false;
        }

        private void SetSchedule(UserSchedule schedule)
        {
            Schedule = schedule;
        }
    }
}
