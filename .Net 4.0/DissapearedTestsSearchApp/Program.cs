using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DissapearedTestsSearchApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var prevTestList = File.ReadAllLines(@"C:\Users\Egor\Downloads\DSS_DSS_Tests_12675-tests.csv");
            var newTestList = File.ReadAllLines(@"C:\Users\Egor\Downloads\DSS_DSS_Tests_12676-tests.csv");

            var list = prevTestList.Except(newTestList).ToList();
        }
    }
}
