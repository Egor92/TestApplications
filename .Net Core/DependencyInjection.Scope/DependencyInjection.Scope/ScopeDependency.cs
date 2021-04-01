using System;

namespace DependencyInjection.Scope
{
    public interface IScopeDependency : IDisposable
    {

    }

    public class ScopeDependency : IScopeDependency
    {
        public ScopeDependency()
        {
            Console.WriteLine("ScopeDependency.ctor()");
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
            Console.WriteLine("ScopeDependency.Dispose()");
        }
    }
}
