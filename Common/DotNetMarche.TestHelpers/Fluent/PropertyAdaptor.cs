using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Constraints;

namespace Nablasoft.Test.UnitTest.Operators {

	/// <summary>
	/// Is used to use a simple property constraint combined with a different constraint that
	/// equal
	/// </summary>
	public class PropertyAdaptor  {

		private String propertyName;
		private MyConstraintBuilder currentConstraint;


		public PropertyAdaptor(string propertyName, MyConstraintBuilder currentConstraint) {
			this.propertyName = propertyName;
			this.currentConstraint = currentConstraint;
		}

		public MyConstraintBuilder LessThan(IComparable value) {
			return currentConstraint.Push(new PropertyConstraint(propertyName, new LessThanConstraint(value)));
		}

		public MyConstraintBuilder GreaterThan(IComparable value) {
			return currentConstraint.Push(new PropertyConstraint(propertyName, new GreaterThanConstraint(value)));
		}
	}
}
