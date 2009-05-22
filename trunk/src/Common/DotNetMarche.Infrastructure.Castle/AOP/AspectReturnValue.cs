using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    /// <summary>
    /// Wrap the concept of return value.
    /// </summary>
    public class AspectReturnValue
    {
        public Object Original { get; set; }
        public Object WrappedReturnValue { get; set; }

        internal Object GetReturnValue()
        {
            return WrappedReturnValue ?? Original;
        }
    }
}