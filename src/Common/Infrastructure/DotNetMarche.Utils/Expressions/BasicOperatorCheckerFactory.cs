using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Utils.Expressions.Concrete;

namespace DotNetMarche.Utils.Expressions
{
	static class BasicOperatorCheckerFactory {

		public static IOperatorsChecker<String> ForBasicOperator() {
			return new StandardOperatorsChecker();
		}
	}
}