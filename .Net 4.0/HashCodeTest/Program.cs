using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCodeTest
{
    class A
    {
        
    }

    class B
    {
         
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a1 = new A().GetHashCode();
            var a2 = new A().GetHashCode();
            var b = new B().GetHashCode();
        }
    }
}
