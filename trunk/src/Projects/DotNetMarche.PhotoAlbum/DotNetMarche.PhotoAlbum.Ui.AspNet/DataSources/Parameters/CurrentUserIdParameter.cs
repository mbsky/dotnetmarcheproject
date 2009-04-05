using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.Parameters
{
   public class CurrentUserIdParameter : Parameter
   {
      protected override object Evaluate(HttpContext context, System.Web.UI.Control control)
      {
         return UserHelper.GetIdOfCurrentUser();
      }
   }
}
