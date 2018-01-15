using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SubscriptionWithoutUnsubscriptionTestApp
{
    public class A : IDisposable
    {
        private readonly IDisposable _subscription = Disposable.Empty;
        private readonly IDisposable _subscription2 = Disposable.Empty;
        private readonly IDisposable _subscription3 = Disposable.Empty;
        private readonly B _innerB = new B();
        private readonly Random _random = new Random();

        public A(GlobalObject globalObject, B outerB)
        {
            _subscription = _innerB.EventRaised.Subscribe();
            if (globalObject != null)
            {
                _subscription2 = globalObject.GlobalEventRaised.Subscribe();
            }
            if (outerB != null)
            {
                _subscription3 = outerB.EventRaised.Subscribe();
            }
        }

        ~A()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            _subscription.Dispose();
            _subscription2.Dispose();
            _subscription3.Dispose();
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

    public class GlobalObject
    {
        #region GlobalEventRaised

        private readonly Subject<Unit> _globalEventRaised = new Subject<Unit>();

        public IObservable<Unit> GlobalEventRaised
        {
            get { return _globalEventRaised.AsObservable(); }
        }

        #endregion
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            List<A> list = new List<A>();

            var globalObject = new GlobalObject();

            int index = 0;
            while (true)
            {
                for (int i = 0; i < 10000; i++)
                {
                    var a = new A(globalObject, new B());
                    //list.Add(a);
                }

                Console.WriteLine("index={0}", index);
                Console.WriteLine("Memory={0}", GC.GetTotalMemory(true));
                index++;
            }
        }
    }
}
