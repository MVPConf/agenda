using Agenda.Shared.Models;
using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Features.Speeches
{
    public partial class SpeechState : State<SpeechState>
    {
        public List<Speech> Speeches { get; private set; }

        public bool Loading { get; private set; }

        public override void Initialize()
        {
            Loading = false;
            Speeches = new List<Speech>();
        }

        private void StartLoading()
        {
            Loading = true;
        }

        private void FinishLoading()
        {
            Loading = false;
        }

        private void SetSpeeches(List<Speech> speeches)
        {
            Speeches = speeches;
        }
    }
}
