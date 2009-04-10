using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Utils.Expressions;
using DotNetMarche.Utils.Expressions.Concrete;

namespace DotNetMarche.Utils.Expressions
{
	public class PostfixExecutor {

		private IOperatorsChecker<String> opChecker = new StandardOperatorsChecker();

		public Double Execute(String expression) {
			String[] tokens = expression.Split(' ');
			Stack<Double> stack = new Stack<Double>();
			foreach (String token in tokens) {
				if (opChecker.IsBinaryOperator(token)) {
					Double op2 = stack.Pop();
					Double op1 = stack.Pop(); 
					switch (token) {
						case "*":
							stack.Push(op1 * op2);
							break;
						case "-":
							stack.Push(op1 - op2);
							break;
						case "+":
							stack.Push(op1 + op2);
							break;
						case "/":
							stack.Push(op1 / op2);
							break;
					}
				}
				else {
					stack.Push(Double.Parse(token));
				}
			}
			return stack.Pop();
		}
	}
}