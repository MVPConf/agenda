using Agenda.Features.Authentication;
using BlazorState;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Features.Schedule
{
    public partial class ScheduleState
    {
        public class RemoveScheduleHandler : ActionHandler<RemoveScheduleAction>
        {
            private readonly HttpClient _httpClient;
            private readonly IMediator _mediator;

            public RemoveScheduleHandler(IStore store, HttpClient httpClient, IMediator mediator) : base(store)
            {
                _httpClient = httpClient;
                _mediator = mediator;
            }

            ScheduleState ScheduleState => Store.GetState<ScheduleState>();

            public override async Task<Unit> Handle(RemoveScheduleAction action, CancellationToken cancellationToken)
            {
                ScheduleState.StartLoading();

                try
                {
                    var result = await _httpClient.DeleteAsync($"schedules/speech/{action.SpeechId}", cancellationToken);
                    result.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == HttpStatusCode.Forbidden
                        || e.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ScheduleState.FinishLoading();
                        await _mediator.Send(new AuthState.LogoutAction());
                    }
                }

                ScheduleState.FinishLoading();
                return await Unit.Task;
            }
        }
    }
}
