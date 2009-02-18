using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Operators
{
	public abstract class BinaryOperator : Constraint  {

		protected Constraint LeftConstraint {
			get { return leftConstraint; }
			set { leftConstraint = value; }
		}
		protected Constraint leftConstraint;

		protected Constraint RightConstraint {
			get { return rightConstraint; }
			set { rightConstraint = value; }
		}
		protected Constraint rightConstraint;

		public virtual Constraint SetConstraint(Constraint left, Constraint right) {
			leftConstraint = left;
			rightConstraint = right;
			return this;
		}
	}
}