using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.TestHelpers.Fluent.Comparers;
using NUnit.Framework.Constraints;

namespace DotNetMarche.TestHelpers.Fluent.Comparers
{
	class ObjectComparerConstraint : Constraint  {
		private Object objExample = null;

		private List<String> mPropertyToCompare = null;
		public List<String> PropertiesToCompare {
			get { return mPropertyToCompare ?? (mPropertyToCompare = new List<string>()); }
		}

		public ObjectComparerConstraint(object objExample) {
			this.objExample = objExample;
		}

		private String errorMessages = String.Empty;
		private String actualObjectTypeName = String.Empty;
		public override bool Matches(object actual) {
			actualObjectTypeName = actual.GetType().Name;
			EntityComparer comparer = new EntityComparer();
			if (mPropertyToCompare != null)
				comparer.PropertiesToCompare.AddRange(PropertiesToCompare);
			errorMessages = comparer.CompareEntities(objExample, actual);
			return String.IsNullOrEmpty(errorMessages);
		}

		public override void WriteDescriptionTo(MessageWriter writer) {
			writer.WritePredicate(String.Format("Compare {0} with {1} result: {2}",
			                                    objExample.GetType().Name, actualObjectTypeName, 
			                                    String.IsNullOrEmpty(errorMessages) ? "are equal" : errorMessages));
		} 
	}
}