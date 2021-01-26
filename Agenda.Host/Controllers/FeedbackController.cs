using Agenda.Host.Models;
using Agenda.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Agenda.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly AgendaDbContext _agendaDbContext;

        public FeedbackController(AgendaDbContext agendaDbContext)
        {
            _agendaDbContext = agendaDbContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveFeedback([FromBody] CreateFeedback body)
        {
            try
            {
                var userHash = User.Claims.First(c => c.Type == ClaimTypes.Hash).Value;

                if (_agendaDbContext.Feedbacks.AsNoTracking().Any(f => f.UserHash == userHash && f.SpeechId == body.SpeechId))
                {
                    return NoContent();
                }

                var feedback = new Feedback()
                {
                    SpeakerEvaluationScore = body.SpeakerEvaluationScore,
                    SpeechEvaluationScore = body.SpeechEvaluationScore,
                    Notes = body.Notes,
                    SpeechId = body.SpeechId,
                    UserHash = userHash
                };

                _agendaDbContext.Feedbacks.Add(feedback);
                await _agendaDbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
