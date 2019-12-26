using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CertificateAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                    .AddCertificate(options =>
                    {
                        options.Events = new CertificateAuthenticationEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                context.Principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                                context.Success();
                                return Task.CompletedTask;
                            },
                            OnCertificateValidated = context =>
                            {
                                throw new Exception("Boom!");
                            }
                        };
                    });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Connection.ClientCertificate = new X509Certificate2();
                var authenticateResult = await context.AuthenticateAsync();
                if (!authenticateResult.Succeeded)
                {
                    await context.ForbidAsync();
                    return;
                }

                await next();
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("User is authenticated");
            });
        }
    }
}
