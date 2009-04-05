using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic
{
   public static class HtmlExtensions
   {
      /// <summary>
      /// Not completely unobtrusive javascript, but useful to wireup partial rendering from
      /// server code.
      /// </summary>
      /// <param name="urlHelper"></param>
      /// <param name="divId"></param>
      /// <param name="linkText"></param>
      /// <param name="routeName"></param>
      /// <param name="routeValues"></param>
      /// <returns></returns>
      public static String PartialRenderingRouteLink(this UrlHelper urlHelper, string divId, string linkText, string routeName, Object routeValues)
      {
         String s = String.Format(@"<a href=""{0}"" onclick=""javascript: $('#{1}').log('thediv').loadext('{2} #thecontent');return false;"">{3}</a>",
                                    "",
                                    divId,
                                    urlHelper.RouteUrl(routeName, routeValues),
                                    linkText);
         return s;                        
      }
   }
}
