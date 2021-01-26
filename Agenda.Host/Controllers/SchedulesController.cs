using Agenda.Host.Models;
using Agenda.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Agenda.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly AgendaDbContext _agendaDbContext;

        public SchedulesController(AgendaDbContext agendaDbContext)
        {
            _agendaDbContext = agendaDbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSchedules()
        {
            try
            {
                var userHash = User.Claims.First(c => c.Type == ClaimTypes.Hash).Value;

                var speeches = await _agendaDbContext.Speeches
                    .AsNoTracking()
                    .Where(s => s.Schedule != null)
                    .ToListAsync();

                var scheduled = await _agendaDbContext.Schedules.AsNoTracking().Where(s => s.UserId == userHash).Select(s => s.Speech.Title).ToListAsync();
                var keynotes = await _agendaDbContext.Speeches.Where(s => s.ExternalId.StartsWith("KN")).ToListAsync();

                keynotes = keynotes.Where(k => !scheduled.Any(s => s == k.Title)).ToList();

                if (keynotes.Count() > 0)
                {
                    _agendaDbContext.Schedules.AddRange(keynotes.Select(k => new Schedule() { Speech = k, UserId = userHash }));
                    await _agendaDbContext.SaveChangesAsync();
                }

                var scheduledSpeeches = await _agendaDbContext.Schedules
                    .AsNoTracking()
                    .Where(s => s.UserId == userHash && s.Speech.Schedule != null && s.Speech.LinkUrl != null)
                    .Select(s => new Shared.Models.Speech()
                    {
                        Id = s.Speech.Id,
                        Title = s.Speech.Title,
                        Description = s.Speech.Description,
                        Track = s.Speech.Track,
                        Speakers = s.Speech.Speakers,
                        Visible = s.Speech.Visible,
                        JoinUrl = s.Speech.JoinUrl,
                        LinkUrl = s.Speech.LinkUrl,
                        Schedule = s.Speech.Schedule.Value.ToString("dd/MM/yyyy HH':'mm")
                    })
                    .ToListAsync();

                var slotTimes = speeches
                    .Select(s => s.Schedule.Value)
                    .Distinct();

                List<Slot> slots = new List<Slot>();

                foreach(DateTime slotTime in slotTimes)
                {
                    if (scheduledSpeeches.Any(ss => ss.Schedule == slotTime.ToString("dd/MM/yyyy HH':'mm")))
                    {
                        slots.AddRange(scheduledSpeeches.Where(ss => ss.Schedule == slotTime.ToString("dd/MM/yyyy HH':'mm"))
                            .Select(ss => new Slot(slotTime, ss)));
                    }
                    else
                    {
                        slots.Add(new Slot(slotTime));
                    }
                }

                return new JsonResult(new UserSchedule()
                {
                    Slots = slots.ToList()
                });

            } 
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("speech/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try
            {
                var userHash = User.Claims.First(c => c.Type == ClaimTypes.Hash).Value;
                var schedule = await _agendaDbContext.Schedules.FirstOrDefaultAsync(s => s.UserId == userHash && s.Speech.Id == id);

                if (schedule != null)
                {
                    _agendaDbContext.Schedules.Remove(schedule);
                    await _agendaDbContext.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveOrUpdateScheduleBySpeechId([FromBody] CreateSchedule body)
        {
            try
            {
                var userHash = User.Claims.First(c => c.Type == ClaimTypes.Hash).Value;

                var speech = await _agendaDbContext.Speeches.FirstAsync(s => s.Id == body.SpeechId);
                var existingSchedule = await _agendaDbContext.Schedules.FirstOrDefaultAsync(s => s.UserId == userHash && s.Speech.Schedule == speech.Schedule);

                if (existingSchedule != null)
                {
                    _agendaDbContext.Remove(existingSchedule);
                }

                var schedule = new Schedule()
                {
                    Speech = await _agendaDbContext.Speeches.FindAsync(body.SpeechId),
                    UserId = userHash
                };

                _agendaDbContext.Schedules.Add(schedule);
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
