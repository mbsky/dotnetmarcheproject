using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Constraint;

namespace DotNetMarche.Utils.Constraint
{
	public abstract class ConstraintBase : IConstraint
	{
		#region IConstraint Members

		public bool Match(object obj)
		{
			return InnerMatch();
		}

		protected abstract Boolean InnerMatch();

		#endregion
	}
}
