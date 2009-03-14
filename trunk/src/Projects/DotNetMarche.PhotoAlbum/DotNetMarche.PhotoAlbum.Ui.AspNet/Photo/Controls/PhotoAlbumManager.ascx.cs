using System;
using System.IO;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Linq;
namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls
{
   public partial class PhotoAlbumManager : System.Web.UI.UserControl
   {
      protected void Page_Load(object sender, EventArgs e)
      {

      }

      protected void SelectButton_OnClick(object sender, EventArgs e)
      {
         Button b = (Button) sender;
         Guid selectedId = new Guid(b.CommandArgument);
         for (int i = 0; i < grdPhotoAlbum.DataKeys.Count; i++)
         {
            if (selectedId.Equals(grdPhotoAlbum.DataKeys[i].Value))
            {
               grdPhotoAlbum.SelectedIndex = i;
               return;
            }
         }
      }

      protected void btnAddNewElement_Click(object sender, EventArgs e)
      {
         Model.PhotoAlbum album = new Model.PhotoAlbum()
                                     {
                                        CreationDate = DateTime.Now,
                                        Description = txtdescriptionForNewElement.Text,
                                        Name = txtNameForNewElement.Text,
                                        Users = Services.SecurityService.GetUserFromUserId(
                                          (Guid)Membership.GetUser().ProviderUserKey)
                                     };
         if (Services.PhotoManagerService.CreateOrUpdatePhotoAlbum(album))
         {
            grdPhotoAlbum.DataBind();
            txtNameForNewElement.Text = "";
            txtdescriptionForNewElement.Text = "";
         }
      }

      /// <summary>
      /// Upload the photo.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void btnUploadPhoto_Click(object sender, EventArgs e)
      {
         FileUpload upload = (FileUpload)frmEdit.FindControl("upPhoto");
         String tempFileName = Path.ChangeExtension(
            Path.GetTempFileName(),
            Path.GetExtension(upload.FileName));
         upload.SaveAs(tempFileName);
         if (Services.PhotoManagerService.AddPhotoToAlbum(tempFileName, upload.FileName, (Guid)grdPhotoAlbum.SelectedValue))
         {
            frmEdit.DataBind();
         }
      }

      protected void SinglePhoto_DataChanged(object sender, EventArgs e)
      {
         frmEdit.DataBind();
      }
   }
}