using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Falico.Infrastructure;

namespace Falico.Net461.Tests
{
    public static class Bootstrapper
    {
        static Bootstrapper()
        {
            BuildContainer();
        }

        public static IContainer Container { get; private set; }
        public static void BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<FalicoModule>();

            builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly())
                .Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces().InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}
