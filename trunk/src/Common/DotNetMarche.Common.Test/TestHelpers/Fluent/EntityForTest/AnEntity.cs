using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest
{
	class AnEntity {
		public AnEntity(string mPropertyA, string mPropertyB) {
			this.mPropertyA = mPropertyA;
			this.mPropertyB = mPropertyB;
		}

		public String PropertyA {
			get { return mPropertyA; }
			set { mPropertyA = value; }
		}
		private String mPropertyA;

		public String PropertyB {
			get { return mPropertyB; }
			set { mPropertyB = value; }
		}
		private String mPropertyB;
	}
}