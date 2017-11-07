using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetWordsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var streamReader = new StreamReader("input.txt"))
            {
                var words = streamReader.ReadToEnd()
                                        .Split('\n')
                                        .Select(x => x.Trim().Split('.'))
                                        .Where(x => x.Count() > 1)
                                        .Select(x => x[1].Trim())
                                        .Where(x => !string.IsNullOrWhiteSpace(x))
                                        .ToList();
                var wordsString = words.Aggregate(string.Empty, (seed, word) => string.Format("{0}\n{1}", seed, word));
                using (var streamWriter = new StreamWriter("output.txt"))
                {
                    streamWriter.Write(wordsString);
                }
            }
        }
    }
}
