using System;
using System.Reflection;
using ProxyClass.Infrastructure;

namespace ProxyClass.Proxies
{
    public class LoggingProxy : Proxy
    {
        public static T CreateFor<T>(T decorated)
        {
            return CreateFor<T, LoggingProxy>(decorated);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                LogBefore(targetMethod, args);

                var result = targetMethod.Invoke(Decorated, args);

                LogAfter(targetMethod, args, result);
                return result;
            }
            catch (Exception ex) when (ex is TargetInvocationException)
            {
                LogException(ex.InnerException ?? ex, targetMethod);
                throw ex.InnerException ?? ex;
            }
        }

        private void LogException(Exception exception, MethodInfo methodInfo = null)
        {
            Console.WriteLine($"Method {methodInfo?.Name} threw exception:\n{exception}");
        }

        private void LogAfter(MethodInfo methodInfo, object[] args, object result)
        {
            Console.WriteLine($"Method {methodInfo.Name} finished");
        }

        private void LogBefore(MethodInfo methodInfo, object[] args)
        {
            Console.WriteLine($"Method {methodInfo.Name} started");
        }
    }
}
