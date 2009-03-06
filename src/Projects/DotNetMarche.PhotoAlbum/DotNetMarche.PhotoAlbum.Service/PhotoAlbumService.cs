﻿using System;
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

         if (album.EntityState == EntityState.Added)
            context.AddToPhotoAlbum(album);
         else
            context.Attach(album);
         return context.SaveChanges() > 0;
      }

      public bool AddPhotoToAlbum(string fileName, String originalFileName, Guid albumId)
      { 
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         Model.PhotoAlbum album = (Model.PhotoAlbum) context.GetObjectByKey(
            context.CreateKeyFor<Model.PhotoAlbum>(albumId));
         Photo photo = PhotoPostProcessor.ProcessPhoto(fileName, originalFileName);
         photo.PhotoAlbum = album;
         album.Photo.Load();
         photo.PhotoIndex = album.Photo.Count() + 1;
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

      public Model.PhotoAlbum GetPhotoAlbumWithPhoto(Guid albumId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         Object entity;
         if (!context.TryGetObjectByKey(context.CreateKeyFor<Model.PhotoAlbum>(albumId), out entity))
            return null;

         Model.PhotoAlbum album = (Model.PhotoAlbum) entity;
         album.Photo.Load();
         album.Photo.ReorderEntities(p => p.PhotoIndex);
         return album;
      }

      #endregion



      #region IPhotoAlbumService Members


      public bool MovePhotoBack(Guid photoId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         Photo photo = (Photo) context.GetObjectByKey(context.CreateKeyFor<Photo>(photoId));
         if (photo.PhotoIndex < 2) return false;
         photo.PhotoAlbumReference.Load();
         Int32 otherPhotoIndex = photo.PhotoIndex - 1;
         Photo photoAtTheLeft = (from Photo p in context.Photo
                                 where p.PhotoIndex == otherPhotoIndex &&
                                       p.PhotoAlbum.Id  == photo.PhotoAlbum.Id 
                                 select p).First();
         photoAtTheLeft.PhotoIndex++;
         photo.PhotoIndex--;
         return true;
      }

      public bool MovePhotoForward(Guid photoId)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         Photo photo = (Photo)context.GetObjectByKey(context.CreateKeyFor<Photo>(photoId));
         photo.PhotoAlbumReference.Load();
         photo.PhotoAlbum.Photo.Load();
         if (photo.PhotoIndex > photo.PhotoAlbum.Photo.Count - 1 ) return false;
         Int32 otherPhotoIndex = photo.PhotoIndex + 1;
         Photo photoAtTheLeft = (from Photo p in context.Photo
                                 where p.PhotoIndex == otherPhotoIndex &&
                                       p.PhotoAlbum.Id == photo.PhotoAlbum.Id
                                 select p).First();
         photoAtTheLeft.PhotoIndex--;
         photo.PhotoIndex++;
         return true;
      }

      public bool ChangePhotoDescription(Guid photoId, string newDescription)
      {
         Model.PhotoAlbumEntities context = ContextManager.GetCurrent();
         Photo photo = (Photo)context.GetObjectByKey(context.CreateKeyFor<Photo>(photoId));
         photo.Description = newDescription;
         return true;
      }

      #endregion
   }
}
