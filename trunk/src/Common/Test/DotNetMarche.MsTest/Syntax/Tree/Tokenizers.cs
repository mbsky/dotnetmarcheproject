using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Utils.Expressions;

namespace DotNetMarche.MsTest.Syntax.Tree
{
	internal class DummyTokenizer : ITokenizer<List<IConstraint>, IConstraint>
	{

		#region ITokenizer<List<Constraint>,Constraint> Members

		public List<IConstraint> Tokenize(List<IConstraint> expressionSource)
		{
			return expressionSource;
		}

		#endregion

	}

	internal class DummyTokenConverter : ITokenConverter<IConstraint, IConstraint>
	{

		#region ITokenConverter<Constraint,Constraint> Members

		public IConstraint Convert(IConstraint source)
		{
			return source;
		}

		#endregion
	}
}
