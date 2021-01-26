using System;
using System.Collections.Generic;

namespace Agenda.Shared.Models
{
    public class UserSchedule
    {
        public List<Slot> Slots { get; set; } = new List<Slot>();
    }
}