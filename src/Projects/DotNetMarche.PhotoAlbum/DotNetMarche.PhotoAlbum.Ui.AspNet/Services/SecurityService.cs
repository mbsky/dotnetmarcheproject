using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetMarche.PhotoAlbum.Service;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Services
{
   public static class SecurityService
   {
       private static readonly ISecurityService instance;

       static SecurityService()
      {
         instance = new Service.SecurityService();
      }

      public static void CreateUser(Guid userId)
      {
          instance.CreateUser(userId);
      }

      public static Model.Users GetUserFromUserId(Guid userId)
      {
         return instance.GetUserFromUserId(userId);
      }

   }
}
