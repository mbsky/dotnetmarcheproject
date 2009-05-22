using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest
{
	public class SimpleTwoProps {
		public SimpleTwoProps(string mPropA, int mPropB) {
			this.mPropA = mPropA;
			this.mPropB = mPropB;
		}

		public String PropA {
			get { return mPropA; }
			set { mPropA = value; }
		}
		private String mPropA;

		public Int32 PropB {
			get { return mPropB; }
			set { mPropB = value; }
		}
		private Int32 mPropB;
	}
}