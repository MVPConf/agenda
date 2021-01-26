using Agenda.Providers;
using Agenda.Services;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using BlazorState;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Agenda
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/") });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            builder.Services.AddBlazorState((options) => options.Assemblies = new Assembly[] { typeof(Program).GetTypeInfo().Assembly });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredToast();

            builder.Services.AddScoped<ConfigurationService, ConfigurationService>();

            await builder.Build().RunAsync();
        }
    }
}
