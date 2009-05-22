using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Common.Test.TestHelpers.Fluent.EntityForTest
{
	class ComposedTwoProps {
		public ComposedTwoProps(string mAnotherProps, SimpleTwoProps mComplexProp) {
			this.mAnotherProps = mAnotherProps;
			this.mComplexProp = mComplexProp;
		}

		public String AnotherProps {
			get { return mAnotherProps; }
			set { mAnotherProps = value; }
		}
		private String mAnotherProps;

		public SimpleTwoProps ComplexProp {
			get { return mComplexProp; }
			set { mComplexProp = value; }
		}
		private SimpleTwoProps mComplexProp;
	}
}