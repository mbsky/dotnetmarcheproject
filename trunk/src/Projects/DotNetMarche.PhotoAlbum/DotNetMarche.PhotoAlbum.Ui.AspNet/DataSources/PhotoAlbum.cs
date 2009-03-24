using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Security;
using DotNetMarche.PhotoAlbum.Service.Dto;

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
      
      [DataObjectMethod(DataObjectMethodType.Select)]
      public IList<Model.PhotoAlbum> GetAll(Guid userId, String SortClause, Int32 maximumRows, Int32 startRowIndex)
      {
         return Services.PhotoManagerService.GetAll(userId, SortClause, maximumRows, startRowIndex);
      }

      [DataObjectMethod(DataObjectMethodType.Select)]
      public  IList<PhotoAlbumInfo> SearchAlbum(String name, String description, String user, String sortClause, Int32 maximumRows, Int32 startRowIndex)
      {
         return Services.PhotoManagerService.SearchAlbum(name, description, user, sortClause, maximumRows, startRowIndex);
      }     
        
      [DataObjectMethod(DataObjectMethodType.Select)]
      public Int32 SearchAlbumGetCount(String name, String description, String user)
      {
         return Services.PhotoManagerService.SearchAlbumGetCount(name, description, user);
      }

      [DataObjectMethod(DataObjectMethodType.Select)]
      public Int32 GetAlbumCount(Guid userId)
      {
         return Services.PhotoManagerService.GetAlbumCount(userId);
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

      /// <summary>
      /// REturn the album with photo.
      /// </summary>
      /// <param name="albumId"></param>
      /// <returns></returns>
      [DataObjectMethod(DataObjectMethodType.Select)]
      public Model.PhotoAlbum GetAlbumWithPhoto(Guid albumId)
      {
         return Services.PhotoManagerService.GetPhotoAlbumWithPhoto(albumId);
      }

      /// <summary>
      /// REturn the album with photo.
      /// </summary>
      /// <param name="albumId"></param>
      /// <returns></returns>
      [DataObjectMethod(DataObjectMethodType.Select)]
      public IList<Model.Photo> GetPhotoForAlbum(Guid albumId)
      {
         return Services.PhotoManagerService.GetAllPhotoForAlbum(albumId);
      }
   }
}
