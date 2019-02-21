using System;
using ProxyClass.Infrastructure;
using ProxyClass.Proxies;

namespace ProxyClass
{
    class Program
    {
        static void Main(string[] args)
        {
            var singer = ProxyBuilder.For<ISinger>()
                                     .ApplyProxy<ISinger, BenchmarkProxy>()
                                     .ApplyProxy<ISinger, LoggingProxy>()
                                     .Create(new Singer());
            singer.SingLala();
            Console.WriteLine();

            singer.SingMacarena();
            Console.WriteLine();

            try
            {
                singer.ThrowException();
            }
            catch (Exception e)
            {
            }
            Console.ReadLine();
        }
    }
}
