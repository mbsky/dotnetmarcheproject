using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Security;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;
using DotNetMarche.PhotoAlbum.Service.ContextManagement;
using DotNetMarche.Utils.EntityFramework;

namespace DotNetMarche.PhotoAlbum.Service
{
   public class SecurityService : ISecurityService
   {

      #region ISecurityService Members

      public void CreateUser(Guid userId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         if (context.TryLoadByKey<Model.Users>(userId) != null)
            throw new SecurityException("Trying to create a user that already exists.");
         Users users = Users.CreateUsers(userId, String.Empty);
         context.AddToUsers(users);
         context.SaveChanges();
      }

      public Model.Users GetUserFromUserId(Guid userId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         EntityKey key3 = context.CreateKeyFor<Users>(userId);
         return context.LoadByKey<Model.PhotoAlbumEntities, Users>(userId);

      }

      #endregion
   }
}
