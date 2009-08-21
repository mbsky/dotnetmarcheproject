using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Syntax.Operators
{
	class OrConstraint: BinaryConstraint
	{
		public OrConstraint() 
			: base((left, right) => left || right)
		{
		}


	}
}
