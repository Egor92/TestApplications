using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CastNullTest
{
    class A {}
    class B {}

    class Program
    {
        static void Main(string[] args)
        {
            object a = null;
            var b = (B) a;

            object i = new int?(4);
            object j = (int)i;
            Console.Read();
        }
    }
}
