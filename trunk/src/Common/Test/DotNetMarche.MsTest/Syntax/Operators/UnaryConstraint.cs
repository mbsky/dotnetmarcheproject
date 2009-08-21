using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Syntax.Operators
{
	public abstract class UnaryConstraint : AbstractConstraint
	{
		public IConstraint SetConstraint(IConstraint constraint)
		{
			throw new NotImplementedException();
		}
	}
}
