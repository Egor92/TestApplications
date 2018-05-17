using System;
using System.Collections.Generic;
using System.Linq;

namespace NthMostRare
{
    internal class Program
    {
        public static int NthMostRare(int[] elements, int n)
        {
            if (n < 0)
                throw new ArgumentException("'n' should be positive", "n");

            var countByElement = new Dictionary<int, int>();
            foreach (var element in elements)
            {
                if (!countByElement.ContainsKey(element))
                {
                    countByElement[element] = 1;
                }
                else
                {
                    countByElement[element]++;
                }
            }

            if (n > countByElement.Count)
                throw new Exception(string.Format("There is no {0} elements in collection", n));

            var pair = countByElement.OrderBy(x => x.Value)
                                     .Take(n)
                                     .Last();
            return pair.Key;
        }

        public static void Main(string[] args)
        {
            int x = NthMostRare(new int[] { 5, 4, 3, 2, 1, 5, 4, 3, 2, 5, 4, 3, 5, 4, 5 }, 2);
            Console.WriteLine(x);
            Console.ReadLine();
        }
    }
}