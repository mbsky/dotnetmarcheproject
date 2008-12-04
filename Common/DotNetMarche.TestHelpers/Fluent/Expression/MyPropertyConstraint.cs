using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Expression
{
	/// <summary>
	/// Class to override standard constraint 
	/// </summary>
	public class MyPropertyConstraint : PrefixConstraint {

		// Fields 
		private string name;
		private bool propertyExists;
		private object propValue;

		// Methods
		public MyPropertyConstraint(string name, Constraint baseConstraint)
			: base(baseConstraint) {
			this.name = name;
			}

		public override bool Matches(object actual) {
			base.actual = actual;
			if (actual == null) {
				return false;
			}
			PropertyInfo property = actual.GetType().GetProperty(this.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			this.propertyExists = property != null;
			if (!propertyExists) {
				return false;
			}
			if (base.baseConstraint == null) {
				return true;
			}
			this.propValue = property.GetValue(actual, null);
			return base.baseConstraint.Matches(this.propValue);
		}

		public override void WriteActualValueTo(MessageWriter writer) {
			if (this.propertyExists) {
				writer.WriteActualValue(this.propValue);
			}
			else {
				writer.WriteActualValue(String.Format("The property {0} was not found on type {1}", name, base.actual.GetType()));
			}
		}

		public override void WriteDescriptionTo(MessageWriter writer) {
			writer.WritePredicate("Property \"" + this.name + "\"");
			base.baseConstraint.WriteDescriptionTo(writer);
		}
	}
}