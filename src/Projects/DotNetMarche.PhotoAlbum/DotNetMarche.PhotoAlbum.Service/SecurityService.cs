using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
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

      public Model.Users GetUserFromUserId(Guid userId)
      {
         using (Model.PhotoAlbumEntities context = new Model.PhotoAlbumEntities())
         {
          //  IEnumerable<KeyValuePair<string, object>> entityKeyValues =
          //new KeyValuePair<string, object>[] {
          //      new KeyValuePair<string, object>("UserId", userId) };

          //  // Create the  key for a specific SalesOrderHeader object. 
          //  EntityKey key = new EntityKey("PhotoAlbumEntities.Users", entityKeyValues);

          //  EntityType type = (from meta in context.MetadataWorkspace.GetItems(DataSpace.CSpace)
          //                     where meta.BuiltInTypeKind == BuiltInTypeKind.EntityType
          //                     select meta)
          //                        .OfType<EntityType>()
          //                        .Where(e => e.Name == typeof(Users).Name).Single();
          //  IEnumerable<KeyValuePair<string, object>> entityKeyValues2 =
          //      type.KeyMembers.Select((k, i) => new KeyValuePair<string, object>(k.Name, userId));

          //  // Create the  key for a specific SalesOrderHeader object. 
          //  EntityKey key2 = new EntityKey(context.GetType().Name + "." + typeof(Users).Name, entityKeyValues2);


          //  return (Users)context.GetObjectByKey(key);
            return context.LoadByKey<Users>(userId);
         }
      }

      #endregion
   }
}
