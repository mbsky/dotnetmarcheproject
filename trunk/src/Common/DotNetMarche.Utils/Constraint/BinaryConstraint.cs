using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Constraint;

namespace DotNetMarche.Utils.Constraint
{
	/// <summary>
	/// The abstract version of a binary constraint.
	/// </summary>
	public abstract class BinaryConstraint : ConstraintBase
	{
		protected IConstraint LeftConstraint
		{
			get { return leftConstraint; }
			set { leftConstraint = value; }
		}
		protected IConstraint leftConstraint;

		protected IConstraint RightConstraint
		{
			get { return rightConstraint; }
			set { rightConstraint = value; }
		}
		protected IConstraint rightConstraint;

		public virtual IConstraint SetConstraint(IConstraint left, IConstraint right)
		{
			leftConstraint = left;
			rightConstraint = right;
			return this;
		}
	}
}
