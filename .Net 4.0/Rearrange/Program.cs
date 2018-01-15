using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace Rearrange
{
    public class Program
    {
        private static void Main(string[] args)
        {
        }

        public static int[] rearrange(int[] elements)
        {
            return elements.OrderBy(GetUnitCountInBinaryRepresentation).ThenBy(x => x).ToArray();
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

    [TestFixture]
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
    }
}