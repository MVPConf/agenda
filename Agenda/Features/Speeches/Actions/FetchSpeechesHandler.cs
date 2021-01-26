using Agenda.Features.Authentication;
using Agenda.Shared.Models;
using BlazorState;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Features.Speeches
{
    public partial class SpeechState
    {
        public class FetchSpeechesHandler : ActionHandler<FetchSpeechesAction>
        {
            private readonly HttpClient _httpClient;
            private readonly IMediator _mediator;

            public FetchSpeechesHandler(IStore store,
                HttpClient httpClient,
                IMediator mediator) : base(store)
            {
                _httpClient = httpClient;
                _mediator = mediator;
            }

            SpeechState State => Store.GetState<SpeechState>();

            public override async Task<Unit> Handle(FetchSpeechesAction action, CancellationToken cancellationToken)
            {
                try
                {
                    State.StartLoading();
                    State.SetSpeeches(await _httpClient.GetFromJsonAsync<List<Speech>>($"speeches", cancellationToken));
                }
                catch (HttpRequestException e)
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
