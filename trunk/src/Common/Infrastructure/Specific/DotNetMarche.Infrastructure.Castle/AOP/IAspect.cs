using System;
using Castle.Core.Interceptor;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    public interface IAspect {
        MethodVoteOptions PreCall(IInvocation invocation);

        void PostCall(IInvocation invocation, AspectReturnValue returnValue);

        void OnException(IInvocation invocation, Exception e, AspectExceptionAction exceptionAction);
    }
}