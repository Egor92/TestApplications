using TrumpSoftware.Common.Enums;
using TrumpSoftware.Common.Helpers;

namespace JustConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = RandomHelper.GetBool();
            var word = RandomStringHelper.GetWord(10, 20, RandomHelper.GetEnumValue<WordCase>());
        }
    }
}