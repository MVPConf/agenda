using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Host.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserHash { get; set; }
        public int SpeechId { get; set; }
        public int SpeechEvaluationScore { get; set; }
        public int SpeakerEvaluationScore { get; set; }
        public string Notes { get; set; }
    }
}
