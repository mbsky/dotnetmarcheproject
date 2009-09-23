using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetMarche.Infrastructure.Helpers
{
	public class GenericIComparer<T> : IComparer<T>
	{

		private readonly MethodBase methodInfo;

		internal GenericIComparer(MethodBase methodInfo, Boolean reversed)
		{
			this.methodInfo = methodInfo;
			mReversed = reversed;
		}

		public GenericIComparer(String propname)
			: this(propname, false)
		{
		}

		public GenericIComparer(String propname, bool mReversed) :
			this(typeof(T).GetProperty(propname).GetGetMethod(), mReversed) { }

		public bool Reversed
		{
			get { return mReversed; }
			set { mReversed = value; }
		}
		private Boolean mReversed = false;

		#region IComparer<T> Members

		public Object GetValueFrom(T obj)
		{
			return methodInfo.Invoke(obj, null);
		}

		public int Compare(T x, T y)
		{
			IComparable obj1 = (IComparable)methodInfo.Invoke(x, null);
			IComparable obj2 = (IComparable)methodInfo.Invoke(y, null);

			Int32 result = (obj1.CompareTo(obj2));
			return Reversed ? -result : result;
		}

		#endregion
	}

	public static class GenericComparerFactory
	{

		private readonly static Dictionary<Type, Dictionary<String, RuntimeMethodHandle>> comparers =
			new Dictionary<Type, Dictionary<string, RuntimeMethodHandle>>();

		public static GenericIComparer<T> GetComparer<T>(string propertyName, bool reversed)
		{
			//Check if the type array for this comparer was created.
			if (!comparers.ContainsKey(typeof(T)))
				comparers.Add(typeof(T), new Dictionary<string, RuntimeMethodHandle>());
			if (!comparers[typeof(T)].ContainsKey(propertyName))
				comparers[typeof(T)].Add(
					propertyName,
					typeof(T).GetProperty(propertyName).GetGetMethod().MethodHandle);
			return (GenericIComparer<T>)new GenericIComparer<T>(
				MethodInfo.GetMethodFromHandle(comparers[typeof(T)][propertyName]), reversed);
		}

		public static GenericIComparer<T> GetComparer<T>(string propertyName)
		{
			return GetComparer<T>(propertyName, false);

		}
	}
}
