using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.TestHelpers.Comparison;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Constraints
{
	public class ObjectEqualConstraint : Constraint
	{
		public Object CompareObj { get; set; }

		public ObjectEqualConstraint(object compareObj)
		{
			CompareObj = compareObj;
		}

		private List<String> differencies;
		public override bool Matches(Object actualObjectToCompare)
		{
			ObjectComparer comparer = new ObjectComparer();
			differencies = comparer.FindDifferencies(actualObjectToCompare, CompareObj);
			return differencies.Count == 0;
		}

		public override void WriteDescriptionTo(MessageWriter writer)
		{
			this.actual = "Found " + differencies.Count + " differencies between two objects";
			differencies.ForEach(s =>
			{
				writer.WriteMessageLine(s);
			});
		}
	}
}
