using System;
using System.Collections.Generic;
using System.Web.Routing;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcHelper;

namespace DotNetMarche.PhotoAlbum.MsUnitTest.MVCUtils
{
    internal class MyTestUrlHelper : IUrlHelper
    {

        #region IUrlHelper Members

        public string RouteUrl(RouteValueDictionary values)
        {
            return RouteUrl((IDictionary<String, Object>)values);
        }

        public string RouteUrl(object values)
        {
            IDictionary<String, Object> dic = (IDictionary<String, Object>)values;
            return "/" + dic["controller"] + "/" + dic["action"];
        }

        public string RouteUrl(string routeName, object values)
        {
            return RouteUrl(values);
        }

        #endregion
    }
}