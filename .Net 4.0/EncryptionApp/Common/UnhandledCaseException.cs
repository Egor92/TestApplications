using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncryptionApp.Common
{
    public class UnhandledCaseException : Exception
    {
        #region Properties

        #region Message

        private string _message;

        public override string Message { get { return _message; }}

        #endregion

        #endregion

        #region Ctor

        public UnhandledCaseException(Type enumType, object value)
        {
            _message = string.Format("Unhandled case of {0}.{1}", enumType, value);
        }

        public UnhandledCaseException(object value)
        {
            if (value == null)
            {
                _message = string.Format("Unhandled case of 'null'");
            }
            else
            {
                _message = string.Format("Unhandled case of {0}.{1}", value.GetType(), value);
            }
        }

        public UnhandledCaseException(string message)
        {
            _message = message;
        }

        public UnhandledCaseException()
        {
        }

        #endregion
    }
}
