using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Expressions.Concrete
{
	public class StringBasicTokenizer : ITokenizer<String, String>
	{
		#region ITokenizer<string,string> Members

		public List<String> Tokenize(string expressionSource)
		{
			return expressionSource.Split(' ').ToList();
		}

		#endregion
	}
}
