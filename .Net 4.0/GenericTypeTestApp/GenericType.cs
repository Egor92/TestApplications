using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace GenericTypeTestApp
{
    public class GenericType : MarkupExtension
    {
        #region Fields

        private const string GenericTypeFormat = "{0}`{1},{2}"; 

        #endregion

        #region Ctor

        public GenericType()
        {
            Generics = new List<Type>();
        }

        #endregion

        #region Properties

        public string ClassName { get; set; }
        public string AssemblyName { get; set; }
        public List<Type> Generics { get; set; }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(ClassName) || string.IsNullOrWhiteSpace(AssemblyName) || !Generics.Any())
                return null;

            var genericType = Type.GetType(string.Format(GenericTypeFormat, ClassName, Generics.Count, AssemblyName));
            if (genericType == null)
                return null;
            
            var result = genericType.MakeGenericType(Generics.ToArray());
            return result;
        }
    }
}
