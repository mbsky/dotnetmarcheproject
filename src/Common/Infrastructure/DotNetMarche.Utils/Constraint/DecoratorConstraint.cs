using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Constraint;

namespace DotNetMarche.Utils.Constraint
{
	public abstract class DecoratorConstraint : ConstraintBase
	{
		protected IConstraint OriginalConstraint
		{
			get { return originalConstraint; }
			set { originalConstraint = value; }
		}
		private IConstraint originalConstraint;

		public void SetConstraint(IConstraint constraint)
		{
			this.originalConstraint = constraint;
		}
	}
}
