using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RowsToColumnConverterApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var streamReader = new StreamReader("input.txt"))
            {
                string[][] originalMatrix = streamReader.ReadToEnd()
                                                        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                                        .Select(x => x.Split(',').ToArray())
                                                        .ToArray();

                int rowCount = originalMatrix.Length;
                int columnCount = originalMatrix.Max(x => x.Length);

                string[][] targetMatrix = new string[columnCount][];
                for (int i = 0; i < columnCount; i++)
                {
                    targetMatrix[i] = new string[rowCount];
                }

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < originalMatrix[i].Length; j++)
                    {
                        targetMatrix[j][i] = originalMatrix[i][j].Trim();
                    }
                }

                using (var streamWriter = new StreamWriter("output.csv", false, Encoding.UTF8))
                {
                    string data = string.Join(Environment.NewLine, targetMatrix.Select(x => string.Join(";", x)));
                    streamWriter.Write(data);
                }
            }
        }
    }
}