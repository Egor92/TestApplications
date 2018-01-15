using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ConvertibleTestApp
{
    class A {}

    class B : A {}

    class Program
    {
        static void Main(string[] args)
        {
            object a = null;
            object i = new int?();
            B b = (B)i;

            var convertedValue = ConvertEx.Convert<int?>(null);
            Console.ReadKey();
        }

    }
}
