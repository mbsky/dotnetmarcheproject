using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1.ExtClasses
{
    class ThrowExceptionISomething : ISomething
    {
        #region ISomething Members

        public string AMethod(string parameter)
        {
            throw new NotImplementedException();
        }

        public string OtherMethod(string parameter, string anotherparameter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}