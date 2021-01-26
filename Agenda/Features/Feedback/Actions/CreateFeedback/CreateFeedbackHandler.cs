using Agenda.Features.Authentication;
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

namespace Agenda.Features.Feedback
{
    public partial class FeedbackState
    {
        public class CreateFeedbackHandler : ActionHandler<CreateFeedbackAction>
        {
            private readonly HttpClient _httpClient;
            private readonly IMediator _mediator;

            public CreateFeedbackHandler(IStore store, HttpClient httpClient, IMediator mediator) : base(store)
            {
                _httpClient = httpClient;
                _mediator = mediator;
            }

            FeedbackState FeedbackState => Store.GetState<FeedbackState>();

            public override async Task<Unit> Handle(CreateFeedbackAction action, CancellationToken cancellationToken)
            {
                FeedbackState.StartLoading();
                try
                {
                    var result = await _httpClient.PostAsJsonAsync("feedback", action.Feedback);
                    result.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException e)
                {
                    if (e.StatusCode == HttpStatusCode.Forbidden
                        || e.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        FeedbackState.FinishLoading();
                        await _mediator.Send(new AuthState.LogoutAction());
                    }
                }

                FeedbackState.FinishLoading();
                return await Unit.Task;
            }
        }
    }
}
