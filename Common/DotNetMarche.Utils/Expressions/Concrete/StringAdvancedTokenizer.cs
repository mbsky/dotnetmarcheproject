using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Expressions.Concrete
{
	public class StringAdvancedTokenizer : ITokenizer<String, String>
	{

		private IOperatorsChecker<String> opChecker;

		private int I;
		private bool isInQuote;
		private string curToken;

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
			List<String> retValue = new List<String>();
			curToken = String.Empty;
			I = 0;
			String opToken;
			isInQuote = false;
			while (I < expressionSource.Length)
			{
				//if next char is separator handle it
				if (char.IsSeparator(expressionSource[I]) && !isInQuote)
				{
					TokenComplete(retValue, ref curToken);
				}
				else if (NextTokenIsOperator(expressionSource, out opToken))
				{
					TokenComplete(retValue, ref curToken);
					retValue.Add(opToken);
					I += opToken.Length - 1;
				}
				else
				{
					//We must check for double quote, if we have a quote go to IsInQuote status where separator gets ignored.
					if (expressionSource[I] == '\'')
					{
						HandleQuote(expressionSource);
					}
					else
					{
						curToken += expressionSource[I];
					}
				}
				++I;
			}
			TokenComplete(retValue, ref curToken);
			return retValue;
		}

		private void HandleQuote(string expressionSource)
		{
			if (isInQuote)
			{
				//It can be the closing quote or a double quote.
				if (I < expressionSource.Length - 2 && expressionSource[this.I + 1] == '\'')
				{
					curToken += '\'';
					I++;
				}
				else
				{
					isInQuote = false;
				}
			}
			else
			{
				isInQuote = true;
			}
		}

		private static void TokenComplete(ICollection<string> retValue, ref string curToken)
		{
			if (!String.IsNullOrEmpty(curToken))
			{
				retValue.Add(curToken);
				curToken = String.Empty;
			}
		}

		/// <summary>
		/// Check if the next token is an operator
		/// </summary>
		/// <param name="I"></param>
		/// <param name="expressionSource"></param>
		/// <param name="operatorToken">Read token</param>
		/// <returns></returns>
		private bool NextTokenIsOperator(string expressionSource, out String operatorToken)
		{
			if (IsSingleCharOperator(expressionSource[I].ToString()))
			{
				operatorToken = expressionSource[I].ToString();
				return true;
			}
			Int32 len = 2;
			String readAhead;
			while (len < 6 && len + I < expressionSource.Length)
			{
				readAhead = expressionSource.Substring(I, len);
				if (opChecker.IsOperator(readAhead))
				{
					operatorToken = readAhead;
					return true;
				}
				len++;
			}

			operatorToken = String.Empty;
			return operatorToken.Length > 0;
		}

		private bool IsSingleCharOperator(string expressionSource)
		{
			return opChecker.IsOperator(expressionSource) ||
				opChecker.IsOpenBracket(expressionSource) ||
				opChecker.IsClosedBracket(expressionSource);
		}

		#endregion
	}
}
