using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest.Comparers
{
	/// <summary>
	/// Copied from Nunit
	/// </summary>
	public static class NumericCompare
	{
		public static Boolean IsNumericType(Object obj)
		{
			return IsFixedPointNumeric(obj) || IsFloatingPointNumeric(obj);
		}

		public static Int32 Compare(Object objl, Object objr)
		{

			if (!IsNumericType(objl) || !IsNumericType(objl))
				throw new ArgumentException( "Both arguments must be numeric");

			if (IsFloatingPointNumeric(objl) || IsFloatingPointNumeric(objr))
				return Convert.ToDouble(objl).CompareTo(Convert.ToDouble(objr));

			if (objl is decimal || objr is decimal)
				return Convert.ToDecimal(objl).CompareTo(Convert.ToDecimal(objr));

			if (objl is ulong || objr is ulong)
				return Convert.ToUInt64(objl).CompareTo(Convert.ToUInt64(objr));

			if (objl is long || objr is long)
				return Convert.ToInt64(objl).CompareTo(Convert.ToInt64(objr));

			if (objl is uint || objr is uint)
				return Convert.ToUInt32(objl).CompareTo(Convert.ToUInt32(objr));

			return Convert.ToInt32(objl).CompareTo(Convert.ToInt32(objr));
 
		}

		/// <summary>
		/// Checks the type of the object, returning true if
		/// the object is a floating point numeric type.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <returns>true if the object is a floating point numeric type</returns>
		public static bool IsFloatingPointNumeric(Object obj)
		{
			if (null != obj)
			{
				if (obj is System.Double) return true;
				if (obj is System.Single) return true;
			}
			return false;
		}

		/// <summary>
		/// Checks the type of the object, returning true if
		/// the object is a fixed point numeric type.
		/// </summary>
		/// <param name="obj">The object to check</param>
		/// <returns>true if the object is a fixed point numeric type</returns>
		public static bool IsFixedPointNumeric(Object obj)
		{
			if (null != obj)
			{
				if (obj is System.Byte) return true;
				if (obj is System.SByte) return true;
				if (obj is System.Decimal) return true;
				if (obj is System.Int32) return true;
				if (obj is System.UInt32) return true;
				if (obj is System.Int64) return true;
				if (obj is System.UInt64) return true;
				if (obj is System.Int16) return true;
				if (obj is System.UInt16) return true;
			}
			return false;
		}
	}
}
