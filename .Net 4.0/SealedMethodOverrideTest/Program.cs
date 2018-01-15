using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SealedMethodOverrideTest
{
    public class A
    {
        public virtual void M()
        {
            Console.WriteLine("A");
        }
    }

    public class B : A
    {
        public override sealed void M()
        {
            Console.WriteLine("B");
        }
    }

    public class C : B
    {
        public new void M()
        {
            Console.WriteLine("C");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var array = new []
            {
                new A(),
                new B(),
                new C(),
            };

            foreach (var a in array)
            {
                a.M();
            }

            Console.ReadKey();
        }
    }
}
