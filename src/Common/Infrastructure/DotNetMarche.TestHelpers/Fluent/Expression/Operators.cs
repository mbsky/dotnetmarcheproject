using System;
using System.Collections.Generic;
using System.Text;
using Nablasoft.Test.UnitTest.Expression;

namespace DotNetMarche.TestHelpers.Fluent.Expression
{
	//public class StandardOperatorsChecker : IOperatorsChecker<String> {
		
	//   private static Dictionary<String, Int32> precedences;
	//   static StandardOperatorsChecker() {
	//      precedences = new Dictionary<String, Int32>();
	//      precedences.Add("+", 10);
	//      precedences.Add("-", 10);
	//      precedences.Add("*", 20);
	//      precedences.Add("/", 20);
	//      precedences.Add("@", 100);
	//      precedences.Add("#", 15);
	//   }

	//   public Boolean IsUnaryOperator(String token) {
	//      return token == "@" || token == "#";
	//   }

	//   public Boolean IsBinaryOperator(String token) {
	//      return precedences.ContainsKey(token);
	//   }

	//   public Boolean OperatorAHasMorePrecedenceThanB(String a, String b) {
	//      return precedences[a] > precedences[b];
	//   }

	//   public bool IsOpenBracket(string token) {
	//      return String.Compare("(", token, true) == 0;
	//   }

	//   public bool IsClosedBracket(string token) {
	//      return String.Compare(")", token, true) == 0;
	//   }

	//}
}