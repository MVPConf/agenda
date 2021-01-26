using Agenda.Host.Models;
using Agenda.Host.Services;
using Agenda.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Agenda.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AgendaDbContext _agendaDbContext;
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configurationService;

        public AuthController(AgendaDbContext agendaDbContext, 
            HttpClient httpClient,
            ConfigurationService configurationService)
        {
            _agendaDbContext = agendaDbContext;
            _httpClient = httpClient;
            _configurationService = configurationService;
        }

        [HttpPost("login/linkedin")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithLinkedinCode([FromBody] AuthRequest auth)
        {
            var clientId = _configurationService.GetClientId();
            var secretId = _configurationService.GetSecretId();
            var linkedinUrl = _configurationService.GetLinkedinUrl();
            var linkedinApi = _configurationService.GetLinkedinApi();

            var response = await _httpClient.PostAsync($"{linkedinUrl}/accessToken?grant_type=authorization_code&code={auth.Code}&redirect_uri={auth.RedirectUri}&client_id={clientId}&client_secret={secretId}", new FormUrlEncodedContent(new Dictionary<string, string>()));

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<OAuthToken>();

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.AccessToken);
                var emailResult = await _httpClient.GetFromJsonAsync<LinkedinEmailResponse>($"{linkedinApi}/emailAddress?q=members&projection=(elements*(handle~))");

                if (emailResult == null || emailResult.Elements.Count == 0)
                {
                    return Unauthorized("Não foi possível realizar login através do Linkedin.<br/>Por favor, limpe o cache do navegador e tente novamente.<br/>Em caso do problema persistir, por favor entre em contato com a organização");
                }

                var user = _agendaDbContext.Users
                    .AsNoTracking()
                    .Include(u => u.Orders)
                    .FirstOrDefault(u => u.Email == emailResult.Elements.First().EmailHandle.Email);

                user.FullName = Regex.Replace(user.FullName, "[^A-Z^a-z^0-9^ ]", "").Trim();

                if (user == null || user.Orders == null || user.Orders.Count == 0)
                {
                    return Unauthorized("Não foi encontrado um ingresso válido para seu usuário.<br/>Acesse <a href=\"https://mvpconf.com.br\">https://mvpconf.com.br</a> e adquira o seu para utilizar a agenda");
                }

                var ticketKeys = user.Orders.Select(o => o.TicketKey).ToList();

                var tickets = await _agendaDbContext.Tickets.AsNoTracking().Where(t => ticketKeys.Contains(t.TicketKey)).ToListAsync();

                return Ok(new OAuthToken(AuthService.GenerateToken(user, tickets, _configurationService.GetJwtSecret()), 86400));
            }
            else
            {
                return BadRequest(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
