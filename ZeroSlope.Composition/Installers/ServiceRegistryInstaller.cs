using System;
using System.Linq;
using Autofac;
using Consul;
using ZeroSlope.Infrastructure.Clients;
using ZeroSlope.Infrastructure.Interfaces;

namespace ZeroSlope.Composition.Installers
{
    public class ServiceRegistryInstaller : IBuilder
    {
        private readonly ContainerOptions _options;

        public ServiceRegistryInstaller(ContainerOptions options)
        {
            _options = options;
        }

        public void Install(ContainerBuilder builder)
        {
            var consulClient = new ConsulClient(c => c.Address = new Uri(_options.Consul.RegisterAddress));

            consulClient.Agent.Services().Result.Response.ToList().ForEach(x => {
                var svcFactory = new ServiceClientFactory(x.Value);
                builder.Register(c => svcFactory);
            });
        }
    }
}
