using System;

namespace DependencyInjection.Scope
{
    public interface ISingleton : IDisposable
    {

    }

    public class Singleton : ISingleton
    {
        public Singleton()
        {
            Console.WriteLine("Singleton.ctor()");
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
            Console.WriteLine("Singleton.Dispose()");
        }
    }
}
