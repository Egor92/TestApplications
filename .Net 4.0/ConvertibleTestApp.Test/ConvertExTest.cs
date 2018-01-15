using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConvertibleTestApp.Test
{
    public class Descendant { }
    public class Ancestor : Descendant { }

    [TestClass]
    public class ConvertExTest
    {
        [TestMethod]
        public void CanConvertObjectToObject()
        {
            CanConvert<object>(new object());
        }

        [TestMethod]
        public void CanConvertNullToObject()
        {
            CanConvert<object>(null);
        }

        [TestMethod]
        public void CanConvertAncestorToDescendant()
        {
            CanConvert<Descendant>(new Ancestor());
        }

        [TestMethod]
        public void CannotConvertDescendantToAncestor()
        {
            CannotConvert<Ancestor>(new Descendant());
        }

        [TestMethod]
        public void CannotConvertNotConvertibleClassToStruct()
        {
            CannotConvert<int>(new Descendant());
        }

        [TestMethod]
        public void CannotConvertStructToNotConvertibleClass()
        {
            CannotConvert<Descendant>(4);
        }

        [TestMethod]
        public void CanConvertDoubleToInt32()
        {
            CanConvert<int>(4.3);
        }

        [TestMethod]
        public void CanConvertInt32ToDouble()
        {
            CanConvert<double>(4);
        }

        [TestMethod]
        public void CanConvertStringToInt32()
        {
            CanConvert<int>("4");
        }

        [TestMethod]
        public void CanConvertStringToDouble()
        {
            CanConvert<double>("4,3");
        }

        [TestMethod]
        public void CanConvertInt32ToString()
        {
            CanConvert<string>(4);
        }

        [TestMethod]
        public void CanConvertDoubleToString()
        {
            CanConvert<string>(4.3);
        }

        [TestMethod]
        public void CanConvertNullableInt32ToString()
        {
            CanConvert<string>(new int?(4));
        }

        [TestMethod]
        public void CanConvertNullableDoubleToString()
        {
            CanConvert<string>(new double?(4));
        }

        [TestMethod]
        public void CanConvertStringToNullableInt32()
        {
            CanConvert<int?>("4");
        }

        [TestMethod]
        public void CanConvertStringToNullableDouble()
        {
            CanConvert<double?>("4,3");
        }

        [TestMethod]
        public void CannotConvertNullToDouble()
        {
            CannotConvert<double>(null);
        }

        [TestMethod]
        public void CannotConvertNullToInt32()
        {
            CannotConvert<int>(null);
        }

        [TestMethod]
        public void CanConvertNullToNullableInt32()
        {
            CanConvert<int?>(null);
        }

        [TestMethod]
        public void CanConvertNullToNullableDouble()
        {
            CanConvert<double?>(null);
        }

        private void CanConvert<T>(object value)
        {
            TestConverting<T>(value, true);
        }

        private void CannotConvert<T>(object value)
        {
            TestConverting<T>(value, false);
        }

        private void TestConverting<T>(object value, bool expectedResult)
        {
            T convertedValue;
            var converted = ConvertEx.TryConvert<T>(value, out convertedValue);
            Assert.AreEqual(expectedResult, converted);
        }
    }
}
