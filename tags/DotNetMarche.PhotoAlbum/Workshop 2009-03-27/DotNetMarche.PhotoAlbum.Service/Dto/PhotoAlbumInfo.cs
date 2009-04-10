using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.PhotoAlbum.Service.Dto
{
   public class PhotoAlbumInfo
   {
      public Guid Id { get; set; }
      public String Name { get; set; }
      public String Description { get; set; }
      public String UserName { get; set; }
      public Guid UserId { get; set; }
   }
}
