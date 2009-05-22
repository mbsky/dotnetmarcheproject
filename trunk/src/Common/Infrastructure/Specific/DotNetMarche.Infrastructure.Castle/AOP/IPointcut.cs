using System;
using System.Reflection;
using Castle.Core.Configuration;
using Castle.Core.Interceptor;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    /// <summary>
    /// This is the interface used to define a pointcut, a point in the execution
    /// code where we want to add an aspect
    /// </summary>
    public interface IPointcut {
        /// <summary>
        /// This method is called to check if a given type contains a valid
        /// pointcut for this definition
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Boolean TypeContainPointcut(Type type);

        /// <summary>
        /// Check if a given method is valid for this pointcut.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Boolean IsPointcutMethod(MethodInfo info);
    }
}