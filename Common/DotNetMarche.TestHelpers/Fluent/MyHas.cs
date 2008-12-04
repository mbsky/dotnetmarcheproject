using System;
using System.Collections.Generic;
using System.Text;

namespace Nablasoft.Test.UnitTest {
	public class MyHas {

		public static MyConstraintBuilder None {
			get {
				return new MyConstraintBuilder().None;
			}
		}

		public static MyConstraintBuilder One {
			get {
				return new MyConstraintBuilder().One; 
			}
		}

		public static MyConstraintBuilder Twice {
			get {
				return new MyConstraintBuilder().Twice;
			}
		}

		public static MyConstraintBuilder All {
			get {
				return new MyConstraintBuilder().All;
			}
		}

		public static MyConstraintBuilder Count(Int32 countOccurrence) {
			return new MyConstraintBuilder().Count(countOccurrence);
		}
	}
}
