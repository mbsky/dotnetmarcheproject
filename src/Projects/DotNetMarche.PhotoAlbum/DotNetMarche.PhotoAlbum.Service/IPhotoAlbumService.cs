using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;
using DotNetMarche.PhotoAlbum.Service.Dto;

namespace DotNetMarche.PhotoAlbum.Service
{
   /// <summary>
   /// Main interface to the photoalbum service
   /// </summary>
   public interface IPhotoAlbumService
   {
      Boolean CreateOrUpdatePhotoAlbum(Model.PhotoAlbum album);

      Boolean AddPhotoToAlbum(String fileName, String originalFileName, Guid albumId);

      Stream GetImage(String imageFileId);

      IList<Model.PhotoAlbum> GetAll(Guid userId);

      IList<Model.PhotoAlbum> GetAll(Guid userId, String sortClause, Int32 maximumRows, Int32 startRowIndex);

      IList<PhotoAlbumInfo> SearchAlbum(String name, String description, String user, String sortClause, Int32 maximumRows, Int32 startRowIndex);

      Int32 SearchAlbumGetCount(String name, String description, String user);

      /// <summary>
      /// Get the count of albums for a current user
      /// </summary>
      /// <param name="userId"></param>
      /// <returns></returns>
      Int32 GetAlbumCount(Guid userId);

      Model.PhotoAlbum GetPhotoAlbumWithPhoto(Guid albumId);

      IList<Model.Photo> GetAllPhotoForAlbum(Guid albumId);

      Boolean MovePhotoBack(Guid photoId);

      Boolean MovePhotoForward(Guid photoId);

      Boolean SwapPhotoPosition(Guid photoId1, Guid photoId2);

      Boolean ChangePhotoDescription(Guid photoId, String newDescription);
     
   }
}
