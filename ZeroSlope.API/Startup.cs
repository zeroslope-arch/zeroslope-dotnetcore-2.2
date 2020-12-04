using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using ZeroSlope.Composition;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ZeroSlope.Service.Filters;
using ZeroSlope.Service.Middleware;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Consul;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Options;
using ZeroSlope.API.Extensions;
using ZeroSlope.API.Filters;

namespace ZeroSlope.Service
{
    #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(env.ContentRootPath, @"Configuration"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"containerOptions.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"containerOptions.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ContainerOptions>(Configuration);

            var settings = Configuration.Get<ContainerOptions>();

            services.AddCors();

            services.AddConsulClient(settings);
            
            services.AddMvcApi();

            services.AddSwashbuckle();

            return services.AddAutofacContainer(settings, container => ApplicationContainer = container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifeTime)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            app.UseSwashbuckle();
            
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());


            app.UseMiddleware<AuthMiddleware>();

            app.UseMvc();

            app.UseStaticFiles();

            app.UseConsul(appLifeTime);

            //app.UseAuthentication();

            appLifeTime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }

    }
}
