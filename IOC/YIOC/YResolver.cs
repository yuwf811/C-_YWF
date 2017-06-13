using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YIOC
{
    public class YResolver
    {
        IContainer _container;

        public YResolver(string jsonPath)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(jsonPath);

            ConfigurationModule configModule = new ConfigurationModule(configBuilder.Build());

            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(configModule);
            _container = containerBuilder.Build();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

    }
}
