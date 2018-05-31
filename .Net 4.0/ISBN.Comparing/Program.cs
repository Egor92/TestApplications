using System.IO;
using System.Linq;

namespace ISBN.Comparing
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines1 = File.ReadAllLines("1618994.txt");
            var lines2 = File.ReadAllLines("1646469.txt");

            var result1 = lines1.Except(lines2).ToList();
            var result2 = lines2.Except(lines1).ToList();

            File.WriteAllLines("Есть в 1618994, нет в 1646469.txt", result1);
            File.WriteAllLines("Есть в 1646469, нет в 1618994.txt", result2);
        }
    }
}
