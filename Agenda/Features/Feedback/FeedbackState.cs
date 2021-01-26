using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Features.Feedback
{
    public partial class FeedbackState : State<FeedbackState>
    {
        public bool Loading { get; private set; }

        public override void Initialize()
        {
            Loading = false;
        }

        private void StartLoading()
        {
            Loading = true;
        }

        private void FinishLoading()
        {
            Loading = false;
        }
    }
}
