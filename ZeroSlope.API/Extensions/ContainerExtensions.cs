using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using ZeroSlope.API.Filters;
using ZeroSlope.Composition;
using ZeroSlope.Service.Filters;

namespace ZeroSlope.API.Extensions
{
    public static class ContainerExtensions
    {
        
        public static IServiceProvider AddAutofacContainer(this IServiceCollection services, ContainerOptions settings, Action<Autofac.IContainer> action)
        {
            var installer = new ContainerInstaller(settings);

            var builder = installer.Install();

            builder.Populate(services);

            var appContainer = builder.Build();

            action(appContainer);

            return new AutofacServiceProvider(appContainer);
        }
    }
}
