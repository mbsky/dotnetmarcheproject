using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest
{
	public class SimpleThreeProps : SimpleTwoProps {

		public SimpleThreeProps(string mPropA, int mPropB, string mThirdProp) : base(mPropA, mPropB) {
			this.mThirdProp = mThirdProp;
		}

		public String ThirdProp {
			get { return mThirdProp; }
			set { mThirdProp = value; }
		}
		private String mThirdProp;
	}
}