using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;
using DotNetMarche.PhotoAlbum.Model.PhotoRoutines;
using DotNetMarche.PhotoAlbum.Service.ContextManagement;
using DotNetMarche.Utils.EntityFramework;

namespace DotNetMarche.PhotoAlbum.Service
{
   public class PhotoAlbumService : IPhotoAlbumService
   {
      #region IPhotoAlbumService Members

      public Boolean CreateOrUpdatePhotoAlbum(Model.PhotoAlbum album)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();

         if (album.EntityState == EntityState.Detached)
            context.AddToPhotoAlbum(album);
         else
            context.Attach(album);
         return context.SaveChanges() > 0;
      }

      public bool AddPhotoToAlbum(string fileName, Guid albumId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         Model.PhotoAlbum album = context.LoadByKey<Model.PhotoAlbum>(albumId);
         Photo photo = PhotoPostProcessor.ProcessPhoto(fileName);
         photo.PhotoAlbum = album;
         context.AddToPhoto(photo);
         return context.SaveChanges() > 0;
      }

      public Stream GetImage(String imageFileId)
      {
         return PhotoFileManager.GetImage(imageFileId);
      }

      public IList<Model.PhotoAlbum> GetAll(Guid userId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         return context.PhotoAlbum
            .Where(pa => pa.Users.UserId == userId).ToList();
      }

      public IList<Model.Photo> GetAllPhotoForAlbum(Guid albumId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         return (from Photo photo in context.Photo
                 where photo.PhotoAlbum.Id == albumId
                 select photo).ToList();
      }

      #endregion


   }
}
