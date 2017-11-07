using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpanFromStringApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Print(TimeSpan.FromSeconds(2));
            Print(TimeSpan.FromSeconds(2000000));
            Print(new TimeSpan(5,6,7,8));
            Print(TimeSpan.Parse(new TimeSpan(5,6,7,8).ToString()));
            Console.Read();
        }

        private static void Print(TimeSpan timeSpan)
        {
            Console.WriteLine(timeSpan);
        }
    }
}
