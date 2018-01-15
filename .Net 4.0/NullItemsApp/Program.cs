using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NullItemsApp
{
    class A {}

    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<A>()
            {
                new A(),
                null,
                new A(),
                null,
                new A(),
                null,
                new A(),
                null,
                new A(),
                null,
            };
            var count = list.Count(x => x == null);
            for (int i = 0; i < count; i++)
            {
                list.Remove(null);
            }
        }
    }
}
