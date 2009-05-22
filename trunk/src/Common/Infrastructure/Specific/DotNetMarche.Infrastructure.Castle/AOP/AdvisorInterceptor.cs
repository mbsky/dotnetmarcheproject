using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.Core.Interceptor;
using Castle.MicroKernel;
using DotNetMarche.Infrastructure.Castle.AOP.Helpers;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    public class AdvisorInterceptor : IInterceptor, IOnBehalfAware {

        private IList<AdviceHandler> advices;
        private IKernel kernel;

        private Dictionary<IntPtr, Boolean> matchlist = new Dictionary<IntPtr, bool>();

        public void Intercept(IInvocation invocation) {
            List<IAspect> handlers = new List<IAspect>();

            foreach (AdviceHandler advice in advices) {
                if (advice.IsPointcutMethod(invocation.Method, kernel)) {
                    handlers.AddRange(advice.CreateAspects(kernel));
                }
            }
            foreach (IAspect handler in handlers) {
                //note that if it halts, the handler is responsbile to set
                //the return value correctly.
                if (handler.PreCall(invocation) == MethodVoteOptions.Halt)
                    return;
            }
            try {
                invocation.Proceed();
           
            }
            catch (Exception e) {
                AspectExceptionAction exceptionAction = new AspectExceptionAction() {IgnoreException=false};
                foreach (IAspect handler in handlers) {
                    handler.OnException(invocation, e, exceptionAction);
                }
                if (!exceptionAction.IgnoreException) throw;
            }
            AspectReturnValue retvalue = new AspectReturnValue() { Original = invocation.ReturnValue };
            foreach (IAspect handler in handlers) {
                handler.PostCall(invocation, retvalue);
            }
            invocation.ReturnValue = retvalue.GetReturnValue();
        }

        public void SetInterceptedComponentModel(ComponentModel target) {
            advices = (IList<AdviceHandler>)target.ExtendedProperties["advices"];
            kernel = (IKernel)target.ExtendedProperties["kernel"];
        }
    }
}