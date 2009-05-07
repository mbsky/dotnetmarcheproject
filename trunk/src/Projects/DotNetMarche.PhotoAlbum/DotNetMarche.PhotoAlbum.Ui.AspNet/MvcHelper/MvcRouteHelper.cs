using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.MvcHelper
{
   public class MvcRouteHelper : IUrlHelper
   {
      public UrlHelper Helper { get; set; }

      public MvcRouteHelper(UrlHelper helper)
      {
         Helper = helper;
      }

      #region IUrlHelper Members

      public string RouteUrl(System.Web.Routing.RouteValueDictionary values)
      {
         return Helper.RouteUrl(values);
      }

      public String RouteUrl(Object values)
      {
         return Helper.RouteUrl(values);
      }

      public string RouteUrl(string routeName, object values)
      {
         return Helper.RouteUrl(routeName, values);
      }

      #endregion
   }
}
