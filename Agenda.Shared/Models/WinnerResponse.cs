using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class WinnerResponse
    {
        public bool Winner { get; set; }

        public WinnerResponse(bool winner)
        {
            Winner = winner;
        }
    }
}
