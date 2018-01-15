using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatIsHashSet
{
    class Program
    {
        static void Main(string[] args)
        {
            var hashSet = new HashSet<int>();
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
                hashSet.Add(random.Next(0, 9));

            foreach (var value in hashSet)
                Console.WriteLine(value);

            Console.Read();
        }
    }
}
