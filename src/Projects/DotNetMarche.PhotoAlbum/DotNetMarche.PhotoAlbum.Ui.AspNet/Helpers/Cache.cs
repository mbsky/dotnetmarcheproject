using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers.Interfaces;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers
{
   /// <summary>
   /// Shield to permit testing.
   /// </summary>
   public static class Cache
   {
      public static T Get<T>(String key)
      {
         if (HttpContext.Current != null && HttpContext.Current.Cache != null)
         {
            return (T) HttpContext.Current.Cache[key];
         }
         return default(T);
      }

      public static void Insert(String key, Object value, CacheDependency dependency, DateTime absoluteExpiration, TimeSpan slidingExpiration)
      {
         if (HttpContext.Current != null && HttpContext.Current.Cache != null)
         {
             HttpContext.Current.Cache.Insert(key, value, dependency, absoluteExpiration, slidingExpiration);
         }
      }

      public static void Insert(String key, Object value, DateTime absoluteExpiration)
      {
         if (HttpContext.Current != null && HttpContext.Current.Cache != null)
         {
            HttpContext.Current.Cache.Insert(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
         }
      }

      public static void Insert(String key, Object value, TimeSpan slidingExpiration)
      {
         if (HttpContext.Current != null && HttpContext.Current.Cache != null)
         {
            HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration);
         }
      }
   }
}
