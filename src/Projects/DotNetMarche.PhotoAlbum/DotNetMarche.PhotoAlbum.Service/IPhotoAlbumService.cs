using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;

namespace DotNetMarche.PhotoAlbum.Service
{
   public interface IPhotoAlbumService
   {
      Model.PhotoAlbum CreatePhotoAlbum(Model.PhotoAlbum album);

      Boolean AddPhotoToAlbum(String fileName, Guid albumId);
   }
}
