using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetMarche.Utils;

namespace DotNetMarche.PhotoAlbum.Model
{
   public partial class PhotoAlbumEntities
   {
      partial void OnContextCreated()
      {
         this.SavingChanges += PhotoAlbumEntities_SavingChanges;
      }

      private void PhotoAlbumEntities_SavingChanges(object sender, EventArgs e)
      {
         foreach (ObjectStateEntry entry in
             ((ObjectContext)sender).ObjectStateManager.GetObjectStateEntries(EntityState.Added))
         {
            if (!entry.IsRelationship)
            {
               PropertyInfo pinfo = entry.Entity.GetType().GetProperty("Id");
               if (pinfo != null)
                  pinfo.SetValue(entry.Entity, GenericUtils.CreateSequentialGuid(), null);
            }


         }

      }
   }
}
