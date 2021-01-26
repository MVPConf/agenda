using Agenda.Providers;
using Agenda.Services;
using Agenda.Shared.Models;
using Blazored.Toast.Services;
using BlazorState;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Features.Authentication
{
    public partial class AuthState
    {
        public class LoginHandler : ActionHandler<LoginAction>
        {
            private readonly AuthenticationStateProvider _authenticationStateProvider;
            private readonly HttpClient _httpClient;
            private readonly ConfigurationService _configuration;

            public LoginHandler(IStore store, 
                AuthenticationStateProvider authenticationStateProvider, 
                HttpClient httpClient,
                ConfigurationService configuration) : base(store)
            {
                _authenticationStateProvider = authenticationStateProvider;
                _httpClient = httpClient;
                _configuration = configuration;
            }

            AuthState State => Store.GetState<AuthState>();

            public override async Task<Unit> Handle(LoginAction action, CancellationToken cancellationToken)
            {
                State.IsLoading = true;
                var redirectUri = _configuration.GetRedirectUri();
                var response = await _httpClient.PostAsJsonAsync<AuthRequest>("auth/login/linkedin", new AuthRequest(action.Code, redirectUri));

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<OAuthToken>();
                    await ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(data.AccessToken);
                    var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                    State.LoggedIn = authState?.User?.Identity?.IsAuthenticated ?? false;
                    State.Email = authState?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    State.Name = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                    State.Hash = authState?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value;
                    State.Role = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "role")?.Value;
                    State.StreamUser = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "stream_user")?.Value;
                    State.StreamPass = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "stream_pass")?.Value;
                    State.ErrorMessage = string.Empty;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
                    || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    State.ErrorMessage = await response.Content.ReadAsStringAsync();
                }

                State.IsLoading = false;

                return await Unit.Task;
            }
        }
    }
}
