using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Extension
{
	public class EnhancedWriter : MessageWriter
	{
		public Object expected;
		public Object actual;
		public Object tolerance;
		public String predicate = " = ";

		public override void DisplayDifferences(object expected, object actual, Tolerance tolerance)
		{
			this.expected = expected;
			this.actual = actual;
			this.tolerance = tolerance;
		}

		public override void DisplayDifferences(object expected, object actual)
		{
			this.expected = expected;
			this.actual = actual;
		}

		public override void DisplayDifferences(Constraint constraint)
		{
			constraint.WriteDescriptionTo(this);
		}

		public override void DisplayStringDifferences(string expected, string actual, int mismatch, bool ignoreCase, bool clipping)
		{
			this.expected = expected;
			this.actual = actual;
		}

		private Int32 maxLineLenght = 1000;
		public override int MaxLineLength
		{
			get
			{
				return maxLineLenght;
			}
			set
			{
				maxLineLenght = value;
			}
		}

		public override void WriteActualValue(object actual)
		{
			this.actual = actual;
		}

		public override void WriteCollectionElements(ICollection collection, int start, int max)
		{
			throw new NotImplementedException();
		}

		public override void WriteConnector(string connector)
		{
			throw new NotImplementedException();
		}

		public override void WriteExpectedValue(object expected)
		{
			this.expected = expected;
		}

		public override void WriteMessageLine(int level, string message, params object[] args)
		{
			throw new NotImplementedException();
		}

		public override void WriteModifier(string modifier)
		{
			throw new NotImplementedException();
		}

		public override void WritePredicate(string predicate)
		{
			this.predicate = predicate;
		}

		public override void WriteValue(object val)
		{
			throw new NotImplementedException();
		}

	}
}
