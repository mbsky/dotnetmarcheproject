using System;
using System.Collections;
using DotNetMarche.MsTest.Comparers;

namespace DotNetMarche.MsTest.Constraints
{
	public class ComparisonConstraintBase : AbstractConstraint
	{
		protected Object Expected { get; set; }
		private Func<Int32, Boolean> CompareFunc;
		private IComparer Comparer = new ObjectComparer();

		public ComparisonConstraintBase(object expected, Func<Int32, Boolean> compareFunc)
		{
			Expected = expected;
			this.CompareFunc = compareFunc;
		}

		internal override bool InnerValidate(object subject)
		{
			return CompareFunc(Comparer.Compare(subject, Expected));
		}
	}
}