using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodVsMethodGroupPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            const int count = 100_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = Enumerable.Range(0, count)
                .Select(x => GetRandom(x))
                .ToList();

            stopwatch.Stop();
            Console.WriteLine("Method: {0}", stopwatch.Elapsed);


            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();

            var list1 = Enumerable.Range(0, count)
                .Select(GetRandom)
                .ToList();

            stopwatch1.Stop();
            Console.WriteLine("Method group: {0}", stopwatch1.Elapsed);

            Console.WriteLine("{0}%", (double)stopwatch.ElapsedTicks / stopwatch1.ElapsedTicks * 100);
        }

        private static double GetRandom(int arg)
        {
            return arg;// Math.Pow(arg, Math.Sin(arg) + Math.Cos(arg));
        }
    }
}
