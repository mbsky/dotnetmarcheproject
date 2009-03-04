using System;
using System.Web.Security;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls
{
   public partial class PhotoAlbumManager : System.Web.UI.UserControl
   {
      protected void Page_Load(object sender, EventArgs e)
      {

      }

      protected void btnAddNewElement_Click(object sender, EventArgs e)
      {
         Model.PhotoAlbum album = new Model.PhotoAlbum()
                                     {
                                        CreationDate = DateTime.Now,
                                        Description = txtdescriptionForNewElement.Text,
                                        Name = txtNameForNewElement.Text,
                                        Users =  Services.SecurityService.GetUserFromUserId(
                                          (Guid) Membership.GetUser().ProviderUserKey)
                                     };
         Services.PhotoManagerService.CreateOrUpdatePhotoAlbum(album);
         grdPhotoAlbum.DataBind();
      }
   }
}