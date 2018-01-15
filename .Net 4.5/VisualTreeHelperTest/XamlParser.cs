using System.Windows.Markup;

namespace VisualTreeHelperTest
{
    internal static class XamlParser
    {
        private static readonly ParserContext ParserContext;

        static XamlParser()
        {
            ParserContext = new ParserContext();
            ParserContext.XmlnsDictionary.Add("", @"http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            ParserContext.XmlnsDictionary.Add("x", @"http://schemas.microsoft.com/winfx/2006/xaml");
            ParserContext.XmlnsDictionary.Add("system", @"clr-namespace:System;assembly=mscorlib");
        }

        internal static T Parse<T>(string xaml)
        {
            return (T)XamlReader.Parse(xaml, ParserContext);
        }
    }
}
