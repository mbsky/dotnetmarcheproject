using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Expressions
{
	/// <summary>
	/// Tokenize an expression
	/// </summary>
	/// <typeparam name="E"></typeparam>
	/// <typeparam name="S"></typeparam>
	public interface ITokenizer<E, S>
	{
		List<S> Tokenize(E expressionSource);
	}
}
