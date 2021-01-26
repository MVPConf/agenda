using Agenda.Host.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Agenda.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadController : ControllerBase
    {
        private readonly AgendaDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public LoadController(AgendaDbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> Load()
        {
            var speeches = await _httpClient.GetFromJsonAsync<List<Shared.Models.SiteSpeech>>("https://mvpconf.com.br/2020/api/speeches");
            var data = speeches.Select(s => new Models.Speech()
            {
                ExternalId = s.Id,
                Track = s.Track,
                Title = s.Title,
                Description = s.Description,
                Visible = s.Visible,
                Speakers = s.Speakers,
                JoinUrl = s.JoinUrl ?? "",
                Schedule = ParseSchedule(s.Scheduler),
                LinkUrl = s.LinkUrl
            }).ToList();

            foreach (var speech in data)
            {
                var dbSpeech = await _dbContext.Speeches.FirstOrDefaultAsync(s => s.ExternalId == speech.ExternalId);

                if (dbSpeech != null)
                {
                    dbSpeech.Track = speech.Track;
                    dbSpeech.Title = speech.Title;
                    dbSpeech.Description = speech.Description;
                    dbSpeech.Visible = speech.Visible;
                    dbSpeech.Speakers = speech.Speakers;
                    dbSpeech.JoinUrl = speech.JoinUrl ?? "";
                    dbSpeech.Schedule = speech.Schedule;
                    dbSpeech.LinkUrl = speech.LinkUrl;

                    _dbContext.Update(dbSpeech);
                }
                else
                {
                    _dbContext.Speeches.Add(speech);
                }
            }

            var dbSpeeches = _dbContext.Speeches.Where(s => !s.ExternalId.StartsWith("KN")).ToList();

            foreach(var dbSpeech in dbSpeeches)
            {
                if (!data.Any(d => d.ExternalId == dbSpeech.ExternalId))
                {
                    var schedulesToRemove = await _dbContext.Schedules.Where(s => s.Speech.Id == dbSpeech.Id).ToListAsync();
                    _dbContext.Schedules.RemoveRange(schedulesToRemove);
                    _dbContext.Speeches.Remove(dbSpeech);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        private DateTime? ParseSchedule(string scheduler)
        {
            DateTime result;
            scheduler = scheduler?.Replace("<br>", "");
            var ok = DateTime.TryParseExact(scheduler, "ddMMyyyy HH':'mm", null, DateTimeStyles.None, out result);

            if (!ok)
            {
                return DateTime.TryParseExact(scheduler, "dd/MM/yyyy HH':'mm", null, DateTimeStyles.None, out result) ? result : null;
            }
            else
            {
                return result;
            }
        }
    }
}