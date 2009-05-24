using System;
using System.Collections.Generic;
using System.Text;

namespace Nablasoft.Test.UnitTest {
	public class MyIs {

		public static MyConstraintBuilder EqualTo(object value) {
			return new MyConstraintBuilder().EqualTo(value);
		}

		public static MyConstraintBuilder LessThan(IComparable value)
		{
			return new MyConstraintBuilder().LessThan(value);
		}

		public static MyConstraintBuilder AllPropertiesEqualTo(object value) {
			return new MyConstraintBuilder().AllPropertiesEqualTo(value);
		}

		public static MyConstraintBuilder SomePropertiesEqualTo(object value, params String[] propertyNames) {
			return new MyConstraintBuilder().SomePropertiesEqualTo(value, propertyNames);
		}

		public static MyConstraintBuilder Not {
			get { return new MyConstraintBuilder().Not;}
		}

		public static MyConstraintBuilder Property(String propertyName, object propertyValue) {
			return new MyConstraintBuilder().Property(propertyName, propertyValue); 
		}
	}
}
