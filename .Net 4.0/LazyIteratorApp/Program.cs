using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazyIteratorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = GetNumbers();
            if (numbers == null)
            {
                Console.WriteLine("A");
            }
            else
            {
                Console.WriteLine("B");
            }
        }

        private static IEnumerable<int> GetNumbers()
        {
            yield return 0;
            yield return 0;
            yield return 0;
            yield return 0;
            yield return 0;
        }
    }
}
