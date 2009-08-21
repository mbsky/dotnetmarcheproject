using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Syntax.Operators
{
	public class AndConstraint : BinaryConstraint
	{
		public AndConstraint() 
			: base((left, right) => left && right)
		{
		}


	}
}
