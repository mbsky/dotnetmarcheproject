using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;

namespace DotNetMarche.Infrastructure.Caching
{
	/// <summary>
	/// This permits to access a global cache component.
	/// </summary>
	public static class GlobalCache
	{
		private static ICache cache;

		#region Override testing routine

		internal static DisposableAction Override(ICache overrideGlobal)
		{
			ICache actualCache = cache;
			cache = overrideGlobal;
			return new DisposableAction(() => cache = actualCache);
		}

		#endregion

		static GlobalCache()
		{
			cache = IoC.Resolve<ICache>();
		}

		#region ICache Members

		public static object Get(object key)
		{
			return cache.Get(key);
		}

		public static T Get<T>(object key)
		{
			return cache.Get<T>(key);
		}

		public static Object Insert(object key, object areaKey, object value, DateTime? absoluteExpirationDate, TimeSpan? slidingExpirationDate)
		{
			return cache.Insert(key, areaKey, value, absoluteExpirationDate, slidingExpirationDate);
		}

		public static void Evict(object key)
		{
			cache.Evict(key);
		}

		public static void EvictArea(object areaKey)
		{
			cache.EvictArea(areaKey);
		}

		public static void Clear()
		{
			cache.Clear();
		}

		#endregion
	}
}
