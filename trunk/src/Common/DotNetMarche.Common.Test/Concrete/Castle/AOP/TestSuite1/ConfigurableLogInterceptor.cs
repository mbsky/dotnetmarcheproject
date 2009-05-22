extern alias OrigCastle;
using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Infrastructure.Castle.AOP;
using Rhino.Mocks;
using CastleIInvocation = OrigCastle::Castle.Core.Interceptor.IInvocation;


namespace DotNetMarche.Common.Test.Concrete.Castle.AOP.TestSuite1
{
    class ConfigurableLogInterceptor : IAspect  {
        #region IAspect Members

        public String ConfigurableProperty {
            get { return mConfigurableProperty; }
            set { mConfigurableProperty = value; }
        }
        private String mConfigurableProperty;

        public MethodVoteOptions PreCall(CastleIInvocation invocation)
        {
            Console.WriteLine("Method called: {0}, Property {1}", invocation.Method.Name, ConfigurableProperty );
            return MethodVoteOptions.Continue; 
        }

        public void PostCall(CastleIInvocation invocation, AspectReturnValue returnValue)
        {
            //override the return value
            returnValue.WrappedReturnValue = "Other";
        }

        public void OnException(CastleIInvocation invocation, Exception e, AspectExceptionAction aspectExceptionAction)
        {
			
        }

        #endregion
    }
}