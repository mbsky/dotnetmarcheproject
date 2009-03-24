using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Parts
{
   public partial class AlbumSearch : System.Web.UI.Page
   {
      protected override void OnInit(EventArgs e)
      {
         Int32 pageIndex = Request.QueryString["page"] != null ? Int32.Parse(Request.QueryString["page"]) -1  : 0;
         String sortClause = Request.QueryString["sort"] ?? "Name";
         txtUser.Text = Request.QueryString["user"];
         txtDescription.Text = Request.QueryString["description"];
         txtName.Text = Request.QueryString["name"];
         grdAlbum.Sort(sortClause, SortDirection.Ascending);
         grdAlbum.PageIndex = pageIndex;

         grdAlbum.DataBind();
         base.OnInit(e);
      }
      protected void Page_Load(object sender, EventArgs e)
      { 
         
      }

      protected String CreateAlbumLink(Object id)
      {
         return "/Photo/AlbumViewer.aspx?albumid=" + id.ToString();
      }

      protected void odsAlbumSearch_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
      {
         //Int32 pageIndex = Request.QueryString["page"] != null ? Int32.Parse(Request.QueryString["page"]) : 0;
         //e.Arguments.StartRowIndex = pageIndex * grdAlbum.PageSize;
         Debug.Write("test");
      }
      protected override void OnPreRender(EventArgs e)
      {
         base.OnPreRender(e);
      }
   }
}
