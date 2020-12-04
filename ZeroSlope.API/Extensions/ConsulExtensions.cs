using System;
using System.Diagnostics;
using System.Linq;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZeroSlope.Composition;

namespace ZeroSlope.API.Extensions
{
    public static class ConsulExtensions
    {
        public static IServiceCollection AddConsulClient(this IServiceCollection services, ContainerOptions settings)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(settings.Consul.RegisterAddress);
            }));

            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            try
            {
                // Read some config
                var settings = app.ApplicationServices.GetRequiredService<IOptions<ContainerOptions>>().Value;
                var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

                // Get server IP address
                var features = app.Properties["server.Features"] as FeatureCollection;
                var addresses = features.Get<IServerAddressesFeature>();
                var address = addresses.Addresses.First();

                // Register service with consul
                var uri = new Uri(address);
                var registration = new AgentServiceRegistration()
                {
                    ID = $"{settings.Consul.ServiceId}-{uri.Port}",
                    Name = settings.Consul.ServiceName,
                    Address = $"{uri.Scheme}://{uri.Host}",
                    Port = uri.Port,
                    Tags = new[] { "ZeroSlope", "Domain", "Gateway" }
                };

                // Deregister if its already registered, and then register
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                consulClient.Agent.ServiceRegister(registration).Wait();

                // If the service shuts down, deregister
                lifetime.ApplicationStopping.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                });
            }
            catch (Exception ex)
            {
                Debugger.Log((int)LogLevel.Warn, "ServiceRegistry", $"Unable to register with Consul: {ex.Message}");
            }

            return app;
        }
    }
}
