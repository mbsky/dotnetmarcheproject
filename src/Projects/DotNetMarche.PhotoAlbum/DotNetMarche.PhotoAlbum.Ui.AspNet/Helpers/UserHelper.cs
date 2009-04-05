using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers
{
   public class UserHelper
   {

      public static Guid GetIdOfCurrentUser()
      {
         return (Guid) Membership.GetUser().ProviderUserKey;
      }
   }
}
