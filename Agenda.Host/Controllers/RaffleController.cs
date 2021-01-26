using Agenda.Host.Models;
using Agenda.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class RaffleController : ControllerBase
    {
        private readonly AgendaDbContext _agendaDbContext;

        public RaffleController(AgendaDbContext agendaDbContext)
        {
            _agendaDbContext = agendaDbContext;
        }

        [HttpGet("check")]
        [Authorize]
        public async Task<IActionResult> CheckResult()
        {
            var userHash = User.Claims.First(c => c.Type == ClaimTypes.Hash).Value;

            var winner = await _agendaDbContext.Winners
                .AsNoTracking()
                .AnyAsync(w => w.UserHash == userHash);

            return Ok(new WinnerResponse(winner));
        }

        [HttpGet("listUsersByDate/{date}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsersOfDate(string date)
        {
            var dateTime = DateTime.ParseExact(date, "yyyyMMdd", null);

            var userHashes = await _agendaDbContext
                .Schedules
                .Include(s => s.Speech)
                .AsNoTracking()
                .Where(s => s.Speech.Schedule != null && s.Speech.Schedule.Value.Date == dateTime)
                .Where(s => !s.Speech.ExternalId.StartsWith("KN"))
                .Select(s => s.UserId)
                .Distinct()
                .ToListAsync();

            var winners = await _agendaDbContext.Winners.AsNoTracking().Select(w => w.UserHash).ToListAsync();

            var users = await _agendaDbContext
                .Users
                .AsNoTracking()
                .Where(u => userHashes.Contains(u.Hash))
                .Where(u => !winners.Contains(u.Hash))
                .Select(u => new UserRaffle(u.Hash, u.FullName, u.Email, u.Company))
                .Distinct()
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("winner")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SaveWinner([FromBody] UserRaffle user)
        {
            var winner = new Winner()
            {
                UserHash = user.UserHash
            };

            _agendaDbContext.Winners.Add(winner);
            await _agendaDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
