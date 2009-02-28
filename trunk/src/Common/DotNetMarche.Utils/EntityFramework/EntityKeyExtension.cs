using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNetMarche.Utils.EntityFramework
{
   public static class EntityKeyExtension
   {




      public static T TryLoadByKey<T>(this ObjectContext context, params Object[] keyValue) where T : EntityObject 
      {
         Object entity;
         if (context.TryGetObjectByKey(context.CreateKeyFor<T>(keyValue), out entity))
            return (T) entity;
         else
            return null;
      }


      public static EntityKey CreateKeyFor<T>(this ObjectContext context, params Object[] keyValue) where T : EntityObject
      {
         EntityType type = (from meta in context.MetadataWorkspace.GetItems(DataSpace.CSpace)
                            where meta.BuiltInTypeKind == BuiltInTypeKind.EntityType
                            select meta)
                                   .OfType<EntityType>()
                                   .Where(e => e.Name == typeof(T).Name).Single();
         IEnumerable<KeyValuePair<string, object>> entityKeyValues =
             type.KeyMembers.Select((k, i) => new KeyValuePair<string, object>(k.Name, keyValue[i]));

         // Create the  key for a specific SalesOrderHeader object. 
         return new EntityKey(context.GetType().Name + "." + typeof(T).Name, entityKeyValues);
      }

      public static T LoadByKey<T>(this ObjectContext context, params Object[] keyValue) where T : EntityObject
      {

         return (T)context.GetObjectByKey(context.CreateKeyFor<T>(keyValue));
      }
   }
}
