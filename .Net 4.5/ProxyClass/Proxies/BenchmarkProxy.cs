using System;
using System.Diagnostics;
using System.Reflection;
using ProxyClass.Infrastructure;

namespace ProxyClass.Proxies
{
    public class BenchmarkProxy : Proxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Stopwatch is started");
            try
            {
                return targetMethod.Invoke(Decorated, args);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Execution time: {stopwatch.ElapsedTicks}");
            }
        }
    }
}
