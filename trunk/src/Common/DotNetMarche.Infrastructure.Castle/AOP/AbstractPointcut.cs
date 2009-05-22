using System;
using System.Reflection;
using Castle.Core.Configuration;
using Castle.Core.Interceptor;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    /// <summary>
    /// This is the abstract class used to implement a pointcut definition
    /// </summary>
    public abstract class AbstractPointcut : IPointcut {
        private IConfiguration configuration;

        public Boolean TypeContainPointcut(Type type) {
            foreach (MethodInfo methodInfo in type.GetMethods()) {
                if (IsPointcutMethod(methodInfo))
                    return true;
            }
            return false;
        }

        #region IPointcut Members

        public abstract Boolean IsPointcutMethod(MethodInfo info);

        #endregion
    }
}