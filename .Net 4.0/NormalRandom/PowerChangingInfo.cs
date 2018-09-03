using System;
using System.Linq;

namespace NormalRandom
{
    public class PowerChangingInfo
    {
        public int ActualValueFrom { get; private set; }
        public int ActualValueTo { get; private set; }
        public int ChangingOffsetFrom { get; private set; }
        public int ChangingOffsetTo { get; private set; }

        public bool IsActualValueInside(int value)
        {
            return ActualValueFrom <= value && value <= ActualValueTo;
        }

        public static PowerChangingInfo Create(string text)
        {
            var values = text.Split(';')
                             .Select(x => x.Trim())
                             .ToArray();

            if (values.Length != 4)
                return null;

            return new PowerChangingInfo
            {
                ActualValueFrom = Convert.ToInt32(values[0]),
                ActualValueTo = Convert.ToInt32(values[1]),
                ChangingOffsetFrom = Convert.ToInt32(values[2]),
                ChangingOffsetTo = Convert.ToInt32(values[3]),
            };
        }
    }
}