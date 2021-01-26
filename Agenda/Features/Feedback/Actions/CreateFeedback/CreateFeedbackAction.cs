using Agenda.Shared.Models;
using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Features.Feedback
{
    public partial class FeedbackState
    {
        public class CreateFeedbackAction : IAction
        {
            public CreateFeedbackAction(Shared.Models.CreateFeedback feedback)
            {
                Feedback = feedback;
            }

            public Agenda.Shared.Models.CreateFeedback Feedback { get; set; }
        }
    }
}
