using System;
using System.Collections.Generic;

namespace ProxyClass.Infrastructure
{
    public class ProxyInfo<T>
    {
        internal readonly IList<Func<T, T>> WrapUpToProxyFuncs = new List<Func<T, T>>();
    }
}
