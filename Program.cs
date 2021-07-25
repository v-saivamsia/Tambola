using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tambola.Constants;
using Tambola.Services;

namespace Tambola
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // custom services
            builder.Services.AddTransient<TicketFactory>();
            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<ComponentService>();
            builder.Services.AddScoped<MarkedWinners>(provider => new MarkedWinners(provider.GetService<ILocalStorageService>()));
            builder.Services.AddScoped<NumberPickerManager>(sp => new NumberPickerManager(sp.GetService<ILocalStorageService>()));
            // helper constant classes
            builder.Services.AddTransient<AvailableWinningWays>();

            // third party services
            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}
