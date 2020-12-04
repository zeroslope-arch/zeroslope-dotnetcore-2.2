using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public static class MvcExtensions
    {
        
        public static IServiceCollection AddMvcApi(this IServiceCollection services)
        {

            services
                .AddMvc(mvcOptions =>
                {
                    mvcOptions.Filters.Add(new ModelstateValidationFilter());
                    mvcOptions.Filters.Add(new HandledResultFilter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                });

            return services;
        }
    }
}
