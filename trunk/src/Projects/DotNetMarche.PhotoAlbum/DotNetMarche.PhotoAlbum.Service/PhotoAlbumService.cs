using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;
using DotNetMarche.PhotoAlbum.Model.PhotoRoutines;
using DotNetMarche.Utils.EntityFramework;

namespace DotNetMarche.PhotoAlbum.Service
{
   public class PhotoAlbumService : IPhotoAlbumService
   {
      #region IPhotoAlbumService Members

      public Boolean CreateOrUpdatePhotoAlbum(Model.PhotoAlbum album)
      {
         using (Model.PhotoAlbumEntities context = new PhotoAlbumEntities())
         {
            if (album.EntityState == EntityState.Detached)
               context.AddToPhotoAlbum(album);
            else
               context.Attach(album);
            return context.SaveChanges() > 0;
         }
      }

      public bool AddPhotoToAlbum(string fileName, Guid albumId)
      {
         using (Model.PhotoAlbumEntities context = new PhotoAlbumEntities())
         {
            Model.PhotoAlbum album = context.LoadByKey<Model.PhotoAlbum>(albumId);
            Photo photo = PhotoPostProcessor.ProcessPhoto(fileName);
            photo.PhotoAlbum = album;
            context.AddToPhoto(photo);
            return context.SaveChanges() > 0;
         }
      }

      public Stream GetImage(String imageFileId)
      {
         return PhotoFileManager.GetImage(imageFileId);
      }

      public IList<Model.PhotoAlbum> GetAll(Guid userId)
      {
          using (Model.PhotoAlbumEntities context = new PhotoAlbumEntities())
          {
             return context.PhotoAlbum
                .Where(pa => pa.Users.UserId == userId).ToList();
          }
      }

      #endregion
   }
}
