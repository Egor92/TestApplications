using System;
using System.Reflection;

namespace ProxyClass.Infrastructure
{
    public abstract class Proxy : DispatchProxy
    {
        protected object Decorated { get; private set; }

        public static T CreateFor<T, TProxy>(T decorated)
            where TProxy : Proxy
        {
            object proxy = Create<T, TProxy>();
            ((Proxy) proxy).SetDecoratedObject(decorated);

            return (T) proxy;
        }

        private void SetDecoratedObject(object decorated)
        {
            Decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        }
    }
}
