using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NestedEnumInXaml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var enumType = typeof (MyClass1.MyClass2.MyClass3.MyClass4.MyClass5.MyEnum);
            var xamlTypeName = GetXamlTypeName(enumType);

        }

        private string GetXamlTypeName(Type type)
        {
            var stringBuilder = new StringBuilder(type.Name);
            while (type.DeclaringType != null)
            {
                stringBuilder.Insert(0, string.Format("{0}+", type.DeclaringType.Name));
                type = type.DeclaringType;
            }
            return stringBuilder.ToString();
        }
    }
}
