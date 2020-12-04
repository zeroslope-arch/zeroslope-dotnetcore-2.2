using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using ZeroSlope.API.Filters;
using ZeroSlope.Composition;

namespace ZeroSlope.API.Extensions
{
    public static class SwashbuckleExtensions
    {
        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZeroSlope");
                c.InjectStylesheet("/swagger-ui/theme.css");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });

            return app;
        }

        public static IServiceCollection AddSwashbuckle(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ZeroSlope", Version = "v1" });
                c.OperationFilter<AuthorizationOperationFilter>();
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", new string[]{ } }
                });
            });

            return services;
        }
    }
}
