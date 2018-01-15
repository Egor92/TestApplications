using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReflectionTest
{
    public class InvokeAttribute : Attribute
    {
    }

    public class A
    {
        [Invoke]
        public object Method1()
        {
            return 5;
        }

        [Invoke]
        public object Method2()
        {
            return 'h';
        }

        [Invoke]
        public object Method3()
        {
            return "Ohhhh";
        }

        [Invoke]
        public object Method4()
        {
            return new object();
        }

        [Invoke]
        public object Method5()
        {
            return 44;
        }

        [Invoke]
        public object Method6()
        {
            return null;
        }

        [Invoke]
        public object Method7()
        {
            return false;
        }

        [Invoke]
        public object Method8()
        {
            return true;
        }

        [Invoke]
        public object Method9()
        {
            return LoaderOptimization.MultiDomain;
        }

        [Invoke]
        public object Method10()
        {
            return NormalizationForm.FormKC;
        }

        [Invoke]
        public object Method11()
        {
            return 15;
        }

        [Invoke]
        public object Method12()
        {
            return string.Empty;
        }

        [Invoke]
        public object Method13()
        {
            return bool.FalseString;
        }

        [Invoke]
        public object Method14()
        {
            return "Volre";
        }

        [Invoke]
        public object Method15()
        {
            return TimeSpan.FromHours(10);
        }

        [Invoke]
        public object Method16()
        {
            return DateTime.Now;
        }

        [Invoke]
        public object Method17()
        {
            return (Int64)790;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var a = new A();
            const int count = 500*20;
            Invoke("Simple call", count, () =>
            {
                a.Method1();
                a.Method2();
                a.Method3();
                a.Method4();
                a.Method5();
                a.Method6();
                a.Method7();
                a.Method8();
                a.Method9();
                a.Method10();
                a.Method11();
                a.Method12();
                a.Method13();
                a.Method14();
                a.Method15();
                a.Method16();
                a.Method17();
            });
            IEnumerable<MethodInfo> methods;
            Invoke("Reflection 1", count, () =>
            {
                methods = typeof (A).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                    .Where(x => x.GetCustomAttributes(typeof (InvokeAttribute), false).Any());
                foreach (var method in methods)
                {
                    method.Invoke(a, null);
                }
            });
            methods = typeof (A).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(x => x.GetCustomAttributes(typeof (InvokeAttribute), false).Any());
            Invoke("Reflection 2", count, () =>
            {
                foreach (var method in methods)
                {
                    method.Invoke(a, null);
                }
            });

            Console.ReadKey();
        }

        private static void Invoke(string description, int count, Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++)
            {
                action();
            }
            stopwatch.Stop();
            var time = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            Console.WriteLine("{0,15}: {1}", description, time);
        }
    }
}