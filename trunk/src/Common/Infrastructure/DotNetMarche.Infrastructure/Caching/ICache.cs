using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Caching
{
	/// <summary>
	/// Base interface for a simple cache.
	/// </summary>
	public interface ICache
	{
		/// <summary>
		/// Gets an object from the cache.
		/// </summary>
		/// <param name="key">The key used to store the object</param>
		/// <returns>the object in cache, null if the object is not in the cache.</returns>
		Object Get(Object key);

		/// <summary>
		/// <see cref="Get"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		T Get<T>(Object key);

		/// <summary>
		/// <see cref="Get"/>
		/// Gets or set a value from the cache.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Object this[Object key] { get;  }

		/// <summary>
		/// Insert an object into the cache.
		/// </summary>
		/// <param name="key">The key used to store the object, if the cache contains already an object
		/// with the same cache the old object gets discharded.</param>
		/// <param name="areaKey">The concept of area is useful to invalidate a bunch of cache objects togheter. Each element
		/// is associated with a key and an areaKey and you can remove object specifing both the key and the areaKey</param>
		/// <param name="value">The object you want to store in the cache.</param>
		/// <param name="absoluteExpirationDate">The absolute time expiration date, the cache can remove the 
		/// item before that time if needed</param>
		/// <param name="slidingExpirationDate">a sliding expiration time.</param>
		/// <returns></returns>
		Object Insert(Object key, Object areaKey, Object value, DateTime? absoluteExpirationDate, TimeSpan? slidingExpirationDate);

		/// <summary>
		/// Evict an object from the cache, if the object is not there nothing gets done.
		/// </summary>
		/// <param name="key"></param>
		void Evict(Object key);

		/// <summary>
		/// Evict all object that belongs to that area key.
		/// </summary>
		/// <param name="areaKey"></param>
		void EvictArea(Object areaKey);

		/// <summary>
		/// Clear all data in the cache.
		/// </summary>
		void Clear();


	}
}
