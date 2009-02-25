using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.PhotoAlbum.Service
{
   public class PhotoAlbumService : IPhotoAlbumService
   {
      #region IPhotoAlbumService Members

      public DotNetMarche.PhotoAlbum.Model.PhotoAlbum CreatePhotoAlbum(DotNetMarche.PhotoAlbum.Model.PhotoAlbum album)
      {
         throw new NotImplementedException();
      }

      public bool AddPhotoToAlbum(string fileName, Guid albumId)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
