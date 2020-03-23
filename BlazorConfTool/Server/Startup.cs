using AutoMapper;
using BlazorConfTool.Server.Hubs;
using BlazorConfTool.Server.Model;
using BlazorConfTool.Shared;
using GrpcGreeter;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Authorization;
using Toolbelt.Extensions.DependencyInjection;

namespace BlazorConfTool.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<ConferencesDbContext>(
                options => options.UseInMemoryDatabase(databaseName: "ConfTool"));

            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ConferenceDetailsValidator>());

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://demo.identityserver.io";
                    options.ApiName = "api";
                });

            services.AddAuthorization(config =>
             {
                 config.AddPolicy("api", builder =>
                 {
                     builder.RequireAuthenticatedUser();
                     builder.RequireScope("api");
                 });
             });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ConfTool API", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    OpenIdConnectUrl = new Uri("https://demo.identityserver.io"),
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri("https://demo.identityserver.io/connect/authorize"),
                            TokenUrl = new Uri("https://demo.identityserver.io/connect/token"),
                            Scopes = new Dictionary<string, string>()
                                {
                                    { "api", "API Access" },
                                },
                        },
                    },
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
                app.UseCssLiveReload();
            }

            app.UseStaticFiles();
            app.UseBlazorFrameworkFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<ConferencesHub>("/conferencesHub");
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapGrpcService<GreeterService>().EnableGrpcWeb();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ConfTool API V1");

                c.OAuthConfigObject = new OAuthConfigObject()
                {
                    ClientId = "interactive.public",
                    ClientSecret = "secret",
                    UsePkceWithAuthorizationCodeGrant = true,
                };
            });
        }
    }
}
