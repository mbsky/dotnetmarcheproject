using System;

namespace DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1.ExtClasses
{
    public interface ISomething {

        String AMethod(String parameter);
        String OtherMethod(String parameter, String anotherparameter);
    }
}