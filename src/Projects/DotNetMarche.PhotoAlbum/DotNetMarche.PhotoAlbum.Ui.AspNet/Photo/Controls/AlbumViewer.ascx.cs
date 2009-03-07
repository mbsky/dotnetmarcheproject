using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Handler;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls
{
   public partial class AlbumViewer : System.Web.UI.UserControl
   {
      protected void Page_Load(object sender, EventArgs e)
      {
         
      }

      protected void ChangePhoto(object sender, EventArgs e)
      {
         ImageButton btn = (ImageButton)sender;
         String photoname = btn.Attributes["photoname"];
         imgMain.ImageUrl = photoname;
      }

      protected String GeneratePhotoUrl(Object fileName)
      {
         return PhotoLoader.GenerateLinkForPhoto((String) fileName);
      }

      protected void odsPhotoAlbum_Selected(object sender, ObjectDataSourceStatusEventArgs e)
      {
         IEnumerable<Model.Photo> retvalue = (IEnumerable<Model.Photo>) e.ReturnValue;
         if (retvalue.Count() == 0) return;
         imgMain.ImageUrl = PhotoLoader.GenerateLinkForPhoto(retvalue.First().FileName);
      }
   }
}