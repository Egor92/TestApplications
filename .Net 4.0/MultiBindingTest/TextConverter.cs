using System;
using System.Globalization;
using Common;

namespace MultiBindingTest
{
    public interface IText
    {
        string Text { get; set; }
    }

    public class TextConverter : ChainConverter<int, string>, IText
    {
        public string Text { get; set; }

        protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value < 0.0)
                return value.ToString();
            if (value > 0.0)
                return Text;
            return "null";
        }

        protected override int ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
