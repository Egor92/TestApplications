using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumerableSequenceEqualTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstArray = new string[]
            {
                "q", "w", "e",
            };
            var secondArray = new string[]
            {
                "w", "e", "q",
            };
            Console.WriteLine(Enumerable.SequenceEqual(firstArray, secondArray));
        }
    }
}
src