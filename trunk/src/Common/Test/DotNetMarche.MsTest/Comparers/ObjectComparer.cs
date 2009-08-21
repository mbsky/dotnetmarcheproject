using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Comparers
{
	public class ObjectComparer : IComparer 
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			if (NumericCompare.IsNumericType(x) && NumericCompare.IsNumericType(y))
				return NumericCompare.Compare(x, y);
			throw new NotImplementedException();
		}


		#endregion
	}
}
