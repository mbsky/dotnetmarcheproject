using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Expressions
{
	/// <summary>
	/// A token converter is able to convert from a token of type S (Source) to 
	/// a token of type D (Destination)
	/// </summary>
	/// <typeparam name="S"></typeparam>
	/// <typeparam name="D"></typeparam>
	public interface ITokenConverter<S, D>
	{
		D Convert(S source);
	}
}
