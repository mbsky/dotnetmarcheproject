using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Expressions.Concrete
{
	public class StringAdvancedTokenizer : ITokenizer<String, String>
	{
		private const int MaxOperatorLength = 6;

		private IOperatorsChecker<String> opChecker;

		private struct AnalysisData
		{
			public int I;
			public bool isInQuote;
			public string curToken;
			public String expressionSource;
			public List<String> tokens;

			public Char CurrentChar
			{
				get { return expressionSource[I]; }
			}
		}



		public StringAdvancedTokenizer(IOperatorsChecker<string> opChecker)
		{
			this.opChecker = opChecker;
		}

		#region ITokenizer<string,string> Members

		/// <summary>
		/// Rule for tokenize is simple, you can separate by space or operator
		/// </summary>
		/// <param name="expressionSource"></param>
		/// <returns></returns>
		public List<string> Tokenize(string expressionSource)
		{
			AnalysisData data = new AnalysisData()
										{
											curToken = String.Empty,
											I = 0,
											isInQuote = false,
											expressionSource = expressionSource,
											tokens = new List<String>()
										};
			String opToken;
			while (data.I < data.expressionSource.Length)
			{
				//if next char is separator handle it
				if (char.IsSeparator(data.CurrentChar) && !data.isInQuote)
				{
					TokenComplete(ref data);
				}
				else if (!data.isInQuote && NextTokenIsOperator(ref data, out opToken))
				{
					TokenComplete(ref data);
					data.tokens.Add(opToken);
					data.I += opToken.Length - 1;
				}
				else
				{
					//We must check for double quote, if we have a quote go to IsInQuote status where separator gets ignored.
					if (data.CurrentChar == '\'')
					{
						HandleQuote(ref data);
					}
					else
					{
						data.curToken += data.CurrentChar;
					}
				}
				++data.I;
			}
			TokenComplete(ref data);
			return data.tokens;
		}

		private void HandleQuote(ref AnalysisData data)
		{
			if (data.isInQuote)
			{
				//It can be the closing quote or a double quote.
				if (data.I < data.expressionSource.Length - 2 && data.expressionSource[data.I + 1] == '\'')
				{
					data.curToken += '\'';
					data.I++;
				}
				else
				{
					data.isInQuote = false;
				}
			}
			else
			{
				data.isInQuote = true;
			}
		}

		private static void TokenComplete(ref AnalysisData data)
		{
			if (!String.IsNullOrEmpty(data.curToken))
			{
				data.tokens.Add(data.curToken);
				data.curToken = String.Empty;
			}
		}

		/// <summary>
		/// Check if the next token is an operator, it should match the longer operator 
		/// that match, this is needed not to miss the != operator.
		/// </summary>
		/// <returns></returns>
		private bool NextTokenIsOperator(ref AnalysisData data, out String operatorToken)
		{
			//First of all we should check if we have parenthesis 
			if (IsOpenOrCloseBracket(data))
			{
				operatorToken = data.CurrentChar.ToString();
				return true;
			}

			//For any operator of the operatorchecker take the bigger that match
			foreach (String op in opChecker.OrderByDescending(s => s.Length))
			{
				if (data.expressionSource.IndexOf(op, data.I, Math.Min(MaxOperatorLength, data.expressionSource.Length - data.I)) == data.I)
				{
					operatorToken = op;
					return true;
				}
			}
			operatorToken = String.Empty;
			return false;
		}

		/// <summary>
		/// Todo, this work only with single char bracket, it should be enough for simple parsers
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private bool IsOpenOrCloseBracket(AnalysisData data)
		{
			return opChecker.IsOpenBracket(data.CurrentChar.ToString()) || opChecker.IsClosedBracket(data.CurrentChar.ToString());
		}

		#endregion
	}
}
