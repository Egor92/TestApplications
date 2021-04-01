using System;
using Unity;
using Unity.Lifetime;

namespace DependencyInjection.Scope
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ISingleton, Singleton>(new ContainerControlledLifetimeManager());
            container.RegisterType<IScopeDependency, ScopeDependency>(new HierarchicalLifetimeManager());

            Console.WriteLine("ORIGINAL CONTAINER");
            container.Resolve<ISingleton>();
            container.Resolve<ISingleton>();
            container.Resolve<IScopeDependency>();
            container.Resolve<IScopeDependency>();

            Console.WriteLine();
            var childContainer = container.CreateChildContainer();

            Console.WriteLine("CHILD CONTAINER");
            childContainer.Resolve<ISingleton>();
            childContainer.Resolve<ISingleton>();
            childContainer.Resolve<IScopeDependency>();
            childContainer.Resolve<IScopeDependency>();

            childContainer.Dispose();
        }
    }
}
