using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Shared.Models
{
    public class CreateFeedback
    {
        public int SpeechId { get; set; }

        [Required(ErrorMessage = "Nota do conteúdo é obrigatória.")]
        [Range(1, 10, ErrorMessage = "Nota do conteúdo é obrigatória.")]
        public int SpeechEvaluationScore { get; set; }

        [Required(ErrorMessage = "Nota do palestrante é obrigatória.")]
        [Range(1, 10, ErrorMessage = "Nota do palestrante é obrigatória.")]
        public int SpeakerEvaluationScore { get; set; }
        
        [StringLength(2000, ErrorMessage = "Limite de 2000 caracteres para observações.")]
        public string Notes { get; set; }
    }
}
