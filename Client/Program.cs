using Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddSyncfusionBlazor();
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient<ImageService>();

            await builder.Build().RunAsync();
        }
    }
}
