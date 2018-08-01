using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NormalRandom
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    if (StartIteration())
                        return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine();
            }
        }

        private static bool StartIteration()
        {
            Console.WriteLine("Press Q to exit");
            Console.WriteLine("Press '1' to generate uniform random numbers");
            Console.WriteLine("Press '2' to generate normal random numbers");
            Console.WriteLine("Press '3' to display random numbers from 0 to 6");

            var input = Console.ReadLine();
            if (input == null || input.Trim().ToUpper() == "Q")
                return true;

            if (input.Trim() == "1")
            {
                Console.Write("Input minimum: ");
                var minimum = Convert.ToInt32(Console.ReadLine());
                Console.Write("Input maximum: ");
                var maximum = Convert.ToInt32(Console.ReadLine());
                Console.Write("Input count: ");
                var count = Convert.ToInt32(Console.ReadLine());
                var result = Enumerable.Range(0, count)
                                       .Select(_ => Rand.GetInt32(minimum, maximum + 1).ToString())
                                       .Aggregate((s1, s2) => string.Format("{0}{1}{2}", s1, Environment.NewLine, s2));

                Clipboard.SetText(result);
                Console.WriteLine("Numbers have been copied to clipboard");
            }

            if (input.Trim() == "2")
            {
                var probabilitiesByValue = File.ReadAllLines("input.txt")
                                               .Select(Rand.GetProbabilitiesByValue)
                                               .Where(x => x != null)
                                               .ToDictionary(x => x.Value.Key, x => x.Value.Value);

                Console.Write("Input count: ");
                var count = Convert.ToInt32(Console.ReadLine());
                var result = Enumerable.Range(0, count)
                                       .Select(_ => Rand.GetValue(probabilitiesByValue).ToString())
                                       .Aggregate((s1, s2) => string.Format("{0}{1}{2}", s1, Environment.NewLine, s2));

                Clipboard.SetText(result);
                Console.WriteLine("Numbers have been copied to clipboard");
            }

            if (input.Trim() == "3")
            {
                Console.Write("Input count: ");
                var count = Convert.ToInt32(Console.ReadLine());
                var result = Enumerable.Range(0, count)
                                       .Select(_ => Rand.GetInt32(0, 7).ToString())
                                       .Aggregate((s1, s2) => string.Format("{0}{1}{2}", s1, Environment.NewLine, s2));

                Console.WriteLine(result);
            }

            return false;
        }
    }
}