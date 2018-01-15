using System.Collections.Generic;
using System.Windows.Data;

namespace MultiBindingTest
{
    public class ValueBindingExtended : ConverterBinding
    {
        public BindingBase Text { get; set; }

        protected override IEnumerable<BindingBase> GetConverterPropertiesBindings()
        {
            yield return Text;
        }

        protected override void SetConverterProperies(IValueConverter converter, object[] propertyValues)
        {
            ConverterBindingHelper.SetPropertyValue<IText, string>(converter, propertyValues[0], (conv, value) => conv.Text = value);
        }
    }
}
