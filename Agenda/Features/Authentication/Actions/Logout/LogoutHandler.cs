using Agenda.Providers;
using BlazorState;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Features.Authentication
{
    public partial class AuthState
    {
        public class LogoutHandler : ActionHandler<LogoutAction>
        {
            private readonly AuthenticationStateProvider _authenticationStateProvider;
            private readonly NavigationManager _navManager;

            public LogoutHandler(IStore store, 
                AuthenticationStateProvider authenticationStateProvider,
                NavigationManager navManager) : base(store)
            {
                _authenticationStateProvider = authenticationStateProvider;
                _navManager = navManager;
            }

            AuthState State => Store.GetState<AuthState>();

            public override async Task<Unit> Handle(LogoutAction action, CancellationToken cancellationToken)
            {
                State.Initialize();
                State.IsLoading = true;
                await ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
                _navManager.NavigateTo("/");
                return await Unit.Task;
            }
        }
    }
}
