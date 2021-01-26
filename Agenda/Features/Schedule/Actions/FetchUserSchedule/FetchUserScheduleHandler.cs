using Agenda.Features.Authentication;
using Agenda.Shared.Models;
using BlazorState;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Features.Schedule
{
    public partial class ScheduleState
    {
        public class FetchUserScheduleHandler : ActionHandler<FetchUserScheduleAction>
        {
            private readonly HttpClient _httpClient;
            private readonly IMediator _mediator;

            public FetchUserScheduleHandler(IStore store,
                HttpClient httpClient,
                IMediator mediator) : base(store)
            {
                _httpClient = httpClient;
                _mediator = mediator;
            }

            ScheduleState State => Store.GetState<ScheduleState>();

            public override async Task<Unit> Handle(FetchUserScheduleAction action, CancellationToken cancellationToken)
            {
                try
                {
                    State.StartLoading();
                    State.SetSchedule(await _httpClient.GetFromJsonAsync<UserSchedule>($"schedules", cancellationToken));
                }
                catch(HttpRequestException e)
                {
                    if (e.StatusCode == HttpStatusCode.Forbidden
                        || e.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        State.FinishLoading();
                        await _mediator.Send(new AuthState.LogoutAction());
                    }
                }

                State.FinishLoading();
                return await Unit.Task;
            }
        }
    }
}
