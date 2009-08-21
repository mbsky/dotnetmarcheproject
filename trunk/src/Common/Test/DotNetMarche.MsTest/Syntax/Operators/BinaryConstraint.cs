using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Syntax.Operators
{
	public abstract class BinaryConstraint : AbstractConstraint
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

		private Func<Boolean, Boolean, Boolean> ComposeFunc;

		protected BinaryConstraint(Func<bool, bool, bool> composeFunc)
		{
			this.leftConstraint = leftConstraint;
			this.rightConstraint = rightConstraint;
			ComposeFunc = composeFunc;
		}

		internal override bool InnerValidate(object subject)
		{
			return ComposeFunc(leftConstraint.Validate(subject), rightConstraint.Validate(subject));
		}


		public virtual IConstraint SetConstraint(IConstraint left, IConstraint right)
		{
			leftConstraint = left;
			rightConstraint = right;
			return this;
		}



	}
}
