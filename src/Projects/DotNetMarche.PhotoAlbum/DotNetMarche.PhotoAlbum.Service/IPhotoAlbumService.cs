using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;

namespace DotNetMarche.PhotoAlbum.Service
{
   /// <summary>
   /// Main interface to the photoalbum service
   /// </summary>
   public interface IPhotoAlbumService
   {
      Boolean CreateOrUpdatePhotoAlbum(Model.PhotoAlbum album);

      Boolean AddPhotoToAlbum(String fileName, Guid albumId);

      Stream GetImage(String imageFileId);

      IList<Model.PhotoAlbum> GetAll(Guid userId);

      IList<Model.Photo> GetAllPhotoForAlbum(Guid albumId);
   }
}
