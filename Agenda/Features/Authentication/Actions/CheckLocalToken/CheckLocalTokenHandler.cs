using BlazorState;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Features.Authentication
{
    public partial class AuthState
    {
        public class CheckLocalTokenHandler : ActionHandler<CheckLocalTokenAction>
        {
            private readonly AuthenticationStateProvider _authenticationStateProvider;

            public CheckLocalTokenHandler(IStore store,
                AuthenticationStateProvider authenticationStateProvider) : base(store)
            {
                _authenticationStateProvider = authenticationStateProvider;
            }

            AuthState State => Store.GetState<AuthState>();

            public override async Task<Unit> Handle(CheckLocalTokenAction action, CancellationToken cancellationToken)
            {
                State.IsLoading = true;

                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                State.LoggedIn = authState?.User?.Identity?.IsAuthenticated ?? false;
                State.Email = authState?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                State.Name = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                State.Hash = authState?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value;
                State.Role = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "role")?.Value;
                State.StreamUser = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "stream_user")?.Value;
                State.StreamPass = authState?.User?.Claims?.FirstOrDefault(c => c.Type == "stream_pass")?.Value;

                State.IsLoading = false;

                return await Unit.Task;
            }
        }
    }
}
