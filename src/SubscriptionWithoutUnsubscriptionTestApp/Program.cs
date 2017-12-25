using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SubscriptionWithoutUnsubscriptionTestApp
{
    public class A : IDisposable
    {
        private readonly IDisposable _subscription;
        private readonly B _b = new B();
        private readonly Random _random = new Random();

        public A()
        {
            _subscription = _b.EventRaised.Subscribe();
        }

        ~A()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseManagedResources();
            }
            ReleaseUnmanagedResources();
        }

        private void ReleaseManagedResources()
        {
            //_subscription.Dispose();
        }

        private void ReleaseUnmanagedResources()
        {
        }
    }

    public class B
    {
        #region EventRaised

        private readonly Subject<Unit> _eventRaised = new Subject<Unit>();

        public IObservable<Unit> EventRaised
        {
            get { return _eventRaised.AsObservable(); }
        }

        #endregion
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            int index = 0;
            while (true)
            {
                for (int i = 0; i < 10000; i++)
                {
                    new A();
                }
                Console.WriteLine("index={0}", index);
                Console.WriteLine("Memory={0}", GC.GetTotalMemory(true));
                index++;
            }
        }
    }
}
