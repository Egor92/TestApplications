using System;
using System.Linq;

namespace ProxyClass.Infrastructure
{
    public static class ProxyBuilder
    {
        public static ProxyInfo<T> For<T>()
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException();

            return new ProxyInfo<T>();
        }

        public static ProxyInfo<T> ApplyProxy<T, TProxy>(this ProxyInfo<T> proxyInfo)
            where TProxy : Proxy, new()
        {
            proxyInfo.WrapUpToProxyFuncs.Add(Proxy.CreateFor<T, TProxy>);
            return proxyInfo;
        }

        public static T Create<T>(this ProxyInfo<T> proxyInfo, T decorated)
        {
            T finalObject = decorated;
            foreach (var wrapUpToProxy in proxyInfo.WrapUpToProxyFuncs.Reverse())
            {
                finalObject = wrapUpToProxy(finalObject);
            }

            return finalObject;
        }
    }
}
