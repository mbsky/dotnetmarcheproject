using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;
using DotNetMarche.Utils.EntityFramework;

namespace DotNetMarche.PhotoAlbum.Service
{
   public class SecurityService : ISecurityService
   {

      #region ISecurityService Members

      public void CreateUser(Guid userId)
      {
         using (Model.PhotoAlbumEntities context = new Model.PhotoAlbumEntities())
         {
            if (context.TryLoadByKey<Model.Users>(userId) != null)
               throw new SecurityException("Trying to create a user that already exists.");
            Users users = Users.CreateUsers(userId, String.Empty);
            context.AddToUsers(users);
            context.SaveChanges();
         }
      }

      #endregion
   }
}
