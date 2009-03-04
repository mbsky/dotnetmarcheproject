using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetMarche.PhotoAlbum.Service;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Services
{
   public static class PhotoManagerService 
   {
      private static readonly IPhotoAlbumService instance;

      static PhotoManagerService()
      {
         instance = new PhotoAlbumService();
      }

      #region IPhotoAlbumService Members

      public static bool CreateOrUpdatePhotoAlbum(Model.PhotoAlbum album)
      {
         return instance.CreateOrUpdatePhotoAlbum(album);
      }

      public static bool AddPhotoToAlbum(string fileName, Guid albumId)
      {
         return instance.AddPhotoToAlbum(fileName, albumId);
      }

      public static System.IO.Stream GetImage(string imageFileId)
      {
         return instance.GetImage(imageFileId);
      }

      public static IList<Model.PhotoAlbum> GetAll(Guid userId)
      {
         return instance.GetAll(userId);
      }

      public static IList<Model.Photo> GetAllPhotoForAlbum(Guid albumId)
      {
         return instance.GetAllPhotoForAlbum(albumId);
      }

      #endregion
   }
}
