using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Caching
{
	/// <summary>
	/// null cache implementation. Null Object Pattern.
	/// </summary>
	public class NullCache : ICache
	{
		#region ICache Members

		public object Get(object key)
		{
			return null;
		}

		public object this[object key]
		{
			get { return Get(key); }
		}

		public void Insert(object key, object areaKey, object value, DateTime absoluteExpirationDate, TimeSpan slidingExpirationDate)
		{
			
		}

		public void Evict(object key)
		{
			
		}

		public void EvictArea(object areaKey)
		{
			
		}

		public void Clear()
		{
			
		}

		#endregion
	}
}
