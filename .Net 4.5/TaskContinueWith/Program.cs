using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskContinueWith
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                Console.WriteLine("1");
                throw new Exception();
                Console.WriteLine("2");
            }).ContinueWith(x =>
            {
                Console.WriteLine("3");
            });
            Console.WriteLine("4");
            Console.Read();
        }
    }
}
