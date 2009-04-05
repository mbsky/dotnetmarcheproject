using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Models
{
   public class AlbumManager : Master
   {
      public IList<Model.PhotoAlbum> Albums { get; set; }
      public Int32? CurrentPage { get; set; }
      public Int32? TotalPages { get; set; }

      public IList<Model.Photo> PhotoForCurrentAlbum { get; set; }
   }
}