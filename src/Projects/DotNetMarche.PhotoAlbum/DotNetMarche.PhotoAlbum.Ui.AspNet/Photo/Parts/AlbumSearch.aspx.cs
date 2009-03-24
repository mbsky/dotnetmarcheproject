using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Parts
{
   public partial class AlbumSearch : System.Web.UI.Page
   {
      protected void Page_Load(object sender, EventArgs e)
      {

      }

      protected String CreateAlbumLink(Object id)
      {
         return "/Photo/AlbumViewer.aspx?albumid=" + id.ToString();
      }
   }
}
