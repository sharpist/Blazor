using Blazor.Server.Data;
using Blazor.Server.Hubs;
using Blazor.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Syncfusion.Blazor;
using System;
using System.IO.Compression;
using System.Linq;

namespace Blazor.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSyncfusionBlazor();
            services.AddControllersWithViews();
            services.AddRazorPages();
            // подключить компрессию
            services.AddResponseCompression(options => {
                options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "text/html+css", "application/octet-stream" });
            });
            // установить уровень сжатия
            services.Configure<BrotliCompressionProviderOptions>(options => {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddDbContext<AppDbContext<EmployeeInfo>>(options =>
            options.UseSqlServer(Configuration["Data:EmployeeDb:ConnectionString"]));
            services.AddTransient<IRepository<EmployeeInfo>, EFRepository<EmployeeInfo>>();

            services.AddDbContext<AppDbContext<ImageData>>(options =>
            options.UseSqlServer(Configuration["Data:ImageDb:ConnectionString"]));
            services.AddTransient<IRepository<ImageData>, EFRepository<ImageData>>();

            services.Configure<InitData>(options => Configuration.GetSection(nameof(InitData)).Bind(options));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IServiceProvider serviceProvider, IOptions<InitData> mySettings)
        {
            // задействовать сжатие
            app.UseResponseCompression();

            if (env.IsDevelopment() || env.IsStaging()) {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();

                Initializer<EmployeeInfo>.EnsurePopulated(serviceProvider,
                    mySettings.Value.Values);
                Initializer<ImageData>.EnsurePopulated(serviceProvider);
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                // built-in API functionality to route-to-code
                endpoints.MapGet("/image", async (context) => {
                    var repository = context.RequestServices.GetService<IRepository<ImageData>>();
                    var images = await repository.TakeAsync();
                    await context.Response.WriteAsJsonAsync(images);
                });
                endpoints.MapPost("/image", async (context) => {
                    if (!context.Request.HasJsonContentType()) {
                        context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                        return;
                    }
                    var repository = context.RequestServices.GetService<IRepository<ImageData>>();
                    var image = await context.Request.ReadFromJsonAsync<ImageData>();
                    await repository.SaveAsync(image);

                    await context.Response.WriteAsJsonAsync(image);
                    context.Response.StatusCode = StatusCodes.Status201Created;
                    return;
                });
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/ChatHub");
                endpoints.MapHub<DashboardHub>("/DashboardHub");
                endpoints.MapHub<EmployeeHub>("/EmployeeHub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
