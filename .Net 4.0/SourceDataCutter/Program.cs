using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SourceDataCutter
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"D:\Data\DSS\DSS-6187\data";
            var fileNames = Directory.EnumerateFiles(path).ToArray();
            for (int i = 0; i < fileNames.Length; i++)
            {
                var fileName = fileNames[i];
                var fileInfo = new FileInfo(fileName);
                var lines = File.ReadAllLines(fileName);
                var finalLines = new List<string>()
                {
                    lines[0],
                };
                finalLines.AddRange(lines.Skip(4150));
                File.WriteAllLines(fileName, finalLines);
                Console.WriteLine("{0,3}/{1,3}: {2}", i, fileNames.Length, fileInfo.Name);
            }
        }
    }
}
