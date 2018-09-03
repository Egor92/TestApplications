using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NormalRandom
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
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
            Console.WriteLine("Press '1' to generate power changing");
            Console.WriteLine("Press '2' to generate normal random numbers");
            Console.WriteLine("Press '3' to display random numbers from 0 to 6");

            var input = Console.ReadLine();
            if (input == null || input.Trim().ToUpper() == "Q")
                return true;

            if (input.Trim() == "1")
            {
                GeneratePowerChanging();
            }

            if (input.Trim() == "2")
            {
                GenerateNormalRandomNumbers();
            }

            if (input.Trim() == "3")
            {
                DisplayRandomNumbers();
            }

            return false;
        }

        private static void GeneratePowerChanging()
        {
            if (!Clipboard.ContainsText())
            {
                Console.Write("Tere is no numbers in clipboard ");
                return;
            }

            var powerChangingInfos = File.ReadAllLines("power-changing.txt")
                                         .Select(PowerChangingInfo.Create)
                                         .Where(x => x != null)
                                         .ToList();

            var newPowerChangings = Clipboard.GetText()
                                             .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                             .Select(x => Convert.ToInt32(x))
                                             .Select(x => powerChangingInfos.FirstOrDefault(p => p.IsActualValueInside(x)))
                                             .Select(x => Rand.GetInt32(x.ChangingOffsetFrom, x.ChangingOffsetTo + 1))
                                             .ToList();

            var result = string.Join(Environment.NewLine, newPowerChangings);
            Clipboard.SetText(result);
            Console.WriteLine("Numbers have been copied to clipboard");
        }

        private static void GenerateNormalRandomNumbers()
        {
            var probabilitiesByValue = File.ReadAllLines("random-impact.txt")
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

        private static void DisplayRandomNumbers()
        {
            Console.Write("Input count: ");
            var count = Convert.ToInt32(Console.ReadLine());
            var result = Enumerable.Range(0, count)
                                   .Select(_ => Rand.GetInt32(0, 7).ToString())
                                   .Aggregate((s1, s2) => string.Format("{0}{1}{2}", s1, Environment.NewLine, s2));

            Console.WriteLine(result);
        }
    }
}