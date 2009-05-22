using System.Collections.Generic;

namespace DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1.ExtClasses
{
    public class SomeClass : ISomething {

        #region ISomething Members

        public string AMethod(string parameter) {
            return "AMethod";
        }

        public string OtherMethod(string parameter, string anotherparameter) {
            return parameter.ToLower() + anotherparameter.ToLower();
        }

        #endregion
    }
}