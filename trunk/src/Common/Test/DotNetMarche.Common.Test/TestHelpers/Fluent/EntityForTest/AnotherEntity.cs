using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest
{
	class AnotherEntity {
		public AnotherEntity(string mPropertyA, Int32 mPropertyB) {
			this.mPropertyA = mPropertyA;
			this.mPropertyB = mPropertyB;
		}

		public String PropertyA {
			get { return mPropertyA; }
			set { mPropertyA = value; }
		}
		private String mPropertyA;

		public Int32 PropertyB {
			get { return mPropertyB; }
			set { mPropertyB = value; }
		}
		private Int32 mPropertyB;
	}
}