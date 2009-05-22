using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    public class RegularExpressionPointcut : AbstractPointcut  {
		
        public IList<Regex> RegularExpressions {
            set { _regularExpressions = value; }
        }
        private IList<Regex> _regularExpressions;

        public override bool IsPointcutMethod(System.Reflection.MethodInfo method) {
            String methodName = method.DeclaringType.FullName + "." + method.Name;
            foreach (Regex rex in _regularExpressions) {
                if (rex.IsMatch(methodName))
                    return true;
            }
            return false;
        }
    }
}