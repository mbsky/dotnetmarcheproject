using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.MvcHelper
{
public interface IUrlHelper
{
   String RouteUrl(RouteValueDictionary values);
   String RouteUrl(Object values); 
   String RouteUrl(String routeName, Object values); 
}
}
