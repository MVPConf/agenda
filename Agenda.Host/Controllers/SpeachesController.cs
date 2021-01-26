using Agenda.Host.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Agenda.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechesController : ControllerBase
    {
        private readonly AgendaDbContext _agendaDbContext;

        public SpeechesController(AgendaDbContext agendaDbContext)
        {
            _agendaDbContext = agendaDbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSpeeches()
        {
            var userHash = User.Claims.First(c => c.Type == ClaimTypes.Hash).Value;

            List<Ticket> tickets = await _agendaDbContext.Tickets
                .FromSqlRaw(@"
                    select t.*
                        from Tickets t with(nolock)
                        inner join Orders o with(nolock) on t.TicketKey = o.TicketKey
                        inner join Users u with(nolock) on u.Id = o.UserId
                        where u.[Hash] = {0}
                ", userHash)
                .AsNoTracking()
                .ToListAsync();

            var speeches = _agendaDbContext.Speeches
                .Include(s => s.Schedules)
                .AsNoTracking();

            if (!tickets.Any(t => t.Description.Contains("PREMIUM") || t.Description.Contains("STANDARD")))
            {
                speeches = speeches.Where(s => s.ExternalId.StartsWith("KN"));
            }

            var speechList = await speeches.Where(s => s.Schedule != null && s.LinkUrl != null).ToListAsync();

            var premium = tickets.Any(t => t.Description.ToUpper().Contains("PREMIUM"));

            return Ok(speechList.Select(s => new Shared.Models.Speech()
            {
                Description = s.Description,
                Id = s.Id,
                JoinUrl = s.JoinUrl,
                LinkUrl = premium ? s.LinkUrl : "",
                Schedule = s.Schedule.Value.ToString("dd/MM/yyyy HH':'mm"),
                Speakers = s.Speakers,
                Title = s.Title,
                Track = s.Track,
                Visible = s.Visible,
                Scheduled = s.Schedules.Count()
            }));
        }
    }
}
