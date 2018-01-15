using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rearrange2
{
    public class Program
    {
        private static void Main(string[] args)
        {
            int[] ints1 = rearrange(new int[]
            {
                5, 5, 3, 7, 10, 14
            });
            int[] ints2 = rearrange(new int[]
            {
                3, 1, 2, 3,
            });
        }

        public static int[] rearrange(int[] elements)
        {
            return elements.Distinct()
                           .OrderBy(GetUnitCountInBinaryRepresentation)
                           .ThenBy(x => x)
                           .ToArray();
        }

        private static int GetUnitCountInBinaryRepresentation(int number)
        {
            int unitCount = 0;
            while (number != 0)
            {
                if ((number & 0x01) == 1)
                    unitCount++;
                number = number >> 1;
            }
            return unitCount;
        }
    }

    /*[TestFixture]
    public class MyTest
    {
        [Test]
        [TestCaseSource("GetSource")]
        public int[] test(int[] elements)
        {
            var result = Program.rearrange(elements);
            return result;
        }

        public IEnumerable<TestCaseData> GetSource()
        {
            yield return new TestCaseData(new int[]
            {
                5, 5, 3, 7, 10, 14
            }).Returns(new int[]
            {
                3, 5, 10, 7, 14,
            });

            yield return new TestCaseData(new int[]
            {
                3, 1, 2, 3,
            }).Returns(new int[]
            {
                1, 2, 3,
            });
        }
    }*/
}