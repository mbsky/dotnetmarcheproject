using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources
{
    [DataObject(true)]
   public class PhotoAlbum
   {
       [DataObjectMethod(DataObjectMethodType.Select)]
       public IList<Model.PhotoAlbum> GetAll(Guid userId)
       {
          return Services.PhotoManagerService.GetAll(userId);
       }

       [DataObjectMethod(DataObjectMethodType.Insert)]
       public Boolean Insert(Model.PhotoAlbum album)
       {
          return Services.PhotoManagerService.CreateOrUpdatePhotoAlbum(album);
       }

       [DataObjectMethod(DataObjectMethodType.Update)]
       public Boolean Update(Model.PhotoAlbum album)
       {
          return Services.PhotoManagerService.CreateOrUpdatePhotoAlbum(album);
       }
   }
}
