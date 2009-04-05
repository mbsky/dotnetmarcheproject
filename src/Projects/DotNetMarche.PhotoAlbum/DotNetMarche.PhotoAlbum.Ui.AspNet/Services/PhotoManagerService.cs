using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetMarche.PhotoAlbum.Service;
using DotNetMarche.PhotoAlbum.Service.Dto;

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

      public static bool AddPhotoToAlbum(string fileName, String originalFileName, Guid albumId)
      {
         return instance.AddPhotoToAlbum(fileName,originalFileName, albumId);
      }

      public static System.IO.Stream GetImage(string imageFileId)
      {
         return instance.GetImage(imageFileId);
      }

      public static IList<Model.PhotoAlbum> GetAll(Guid userId)
      {
         return instance.GetAll(userId);
      }     
      
      public static IList<Model.PhotoAlbum> GetAll(Guid userId, String SortClause, Int32 maximumRows, Int32 startRowIndex)
      {
         return instance.GetAll(userId, SortClause, maximumRows, startRowIndex);
      }

      public static IList<Model.Photo> GetAllPhotoForAlbum(Guid albumId)
      {
         return instance.GetAllPhotoForAlbum(albumId);
      }

      public static Model.PhotoAlbum GetPhotoAlbumWithPhoto(Guid albumId)
      {
         return instance.GetPhotoAlbumWithPhoto(albumId);
      }

      public static Boolean MovePhotoBack(Guid photoId)
   {
      return instance.MovePhotoBack(photoId);
   }

      public static Boolean MovePhotoForward(Guid photoId)
      {
         return instance.MovePhotoForward(photoId);
      }

      public static Boolean ChangePhotoDescription(Guid photoId, String newDescription)
      {
         return instance.ChangePhotoDescription(photoId, newDescription);
      }

      public static  Boolean SwapPhotoPosition(Guid photoId1, Guid photoId2)
      {
         return instance.SwapPhotoPosition(photoId1, photoId2);
      }

      public static int GetAlbumCount(Guid userId)
      {
         return instance.GetAlbumCount(userId);
      }

      public static   IList<PhotoAlbumInfo> SearchAlbum(String name, String description, String user, String sortClause, Int32 maximumRows, Int32 startRowIndex)
      {
         return instance.SearchAlbum(name, description, user, sortClause, maximumRows, startRowIndex);
      }

      public static Int32 SearchAlbumGetCount(String name, String description, String user)
      {
         return instance.SearchAlbumGetCount(name, description, user);
      }

      #endregion


   }
}
