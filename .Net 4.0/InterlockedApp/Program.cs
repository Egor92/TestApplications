using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace InterlockedApp
{
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    internal class Program
    {
        private static readonly object _syncRoot = new object();

        private static void Main(string[] args)
        {
            if (File.Exists("stop"))
                return;

            AppDomain.CurrentDomain.FirstChanceException += AppDomainOnFirstChanceException;

            Parallel.For(0, 3, _ =>
            {
                var points = new List<Point>();
                while (true)
                {
                    points.Add(new Point());
                }
            });
        }

        private static void AppDomainOnFirstChanceException(object sender, FirstChanceExceptionEventArgs eventArgs)
        {
            Exception logException = eventArgs.Exception;

            if (eventArgs.Exception.InnerException != null)
            {
                logException = eventArgs.Exception.GetBaseException();
            }

            if (logException is OutOfMemoryException)
            {
                lock (_syncRoot)
                {
                    Process.Start(Assembly.GetEntryAssembly().Location, "");
                    Process.GetCurrentProcess().Kill();
                }
            }
        }
    }
}