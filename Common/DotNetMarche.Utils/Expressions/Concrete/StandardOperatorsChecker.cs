using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Utils.Expressions;
using System.Linq;

namespace DotNetMarche.Utils.Expressions.Concrete
{
	/// <summary>
	/// Contains standard operators
	/// </summary>
	public class StandardOperatorsChecker : IOperatorsChecker<String> {
		
		private static Dictionary<String, Int32> precedences;
		static StandardOperatorsChecker() {
			precedences = new Dictionary<String, Int32>();
			precedences.Add("+", 10);
			precedences.Add("-", 10);
			precedences.Add("*", 20);
			precedences.Add("/", 20);
			precedences.Add("@", 100);
			precedences.Add("#", 15);
			precedences.Add("==", 15);
			precedences.Add("like", 15);
		}

		public Boolean IsUnaryOperator(String token) {
			return token == "@" || token == "#";
		}

		public Boolean IsBinaryOperator(String token) {
			return precedences.ContainsKey(token);
		}

		public Boolean OperatorAHasMorePrecedenceThanB(String a, String b) {
			return precedences[a] > precedences[b];
		}

		public bool IsOpenBracket(string token) {
			return String.Compare("(", token, true) == 0;
		}

		public bool IsClosedBracket(string token) {
			return String.Compare(")", token, true) == 0;
		}

		public bool IsOperator(string token)
		{
			return precedences.ContainsKey(token);
		}



		#region IEnumerable<string> Members

		public IEnumerator<string> GetEnumerator()
		{
			return precedences.Select(kvp => kvp.Key).GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}