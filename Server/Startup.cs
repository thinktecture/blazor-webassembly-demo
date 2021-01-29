using System;
using System.Linq;
using AutoMapper;
using ConfTool.Server.GrpcServices;
using ConfTool.Server.Hubs;
using ConfTool.Server.Model;
using ConfTool.Server.PrerenderAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProtoBuf.Grpc.Server;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.AspNetCore;
using ConfTool.Shared.Validation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ConfTool.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // For prerendering
            services.AddRemoteAuthentication<RemoteAuthenticationState, RemoteUserAccount, OidcProviderOptions>();
            services.AddScoped<AuthenticationStateProvider, RemoteAuthenticationService>();
            services.AddScoped<SignOutSessionStateManager>();
            services.AddTransient<Microsoft.JSInterop.IJSRuntime, JSRuntime>();
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<ConferencesDbContext>(
                options => options.UseInMemoryDatabase(databaseName: "ConfTool"));

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ConferenceDetailsValidator>());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    Configuration.Bind("Oidc", options);
                    options.RefreshOnIssuerKeyNotFound = true;
                });

            services.AddAuthorization(config =>
             {
                 config.AddPolicy("api", builder =>
                 {
                     builder.RequireAuthenticatedUser();
                     builder.RequireScope("api");
                 });
             });
             
            services.AddSignalR().AddMessagePackProtocol();

            services.AddGrpc();
            services.AddCodeFirstGrpc(config => { config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal; });

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseGrpcWeb();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>().EnableGrpcWeb();
                endpoints.MapGrpcService<CounterService>().EnableGrpcWeb();
                endpoints.MapGrpcService<TimeService>().EnableGrpcWeb();
                endpoints.MapGrpcService<ConferencesServiceCodeFirst>().EnableGrpcWeb();

                endpoints.MapHub<ConferencesHub>("/conferencesHub");

                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
