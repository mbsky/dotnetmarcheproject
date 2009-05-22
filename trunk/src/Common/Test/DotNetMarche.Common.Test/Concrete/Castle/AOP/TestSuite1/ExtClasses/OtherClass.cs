using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1.ExtClasses
{
    public class OtherClass : IAnotherSomething {

        #region IAnotherSomething Members

        public string AnotherMethod() {
            return "TEST";
        }

        #endregion
    }
}