using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Handler;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls
{
   public partial class SinglePhotoThumbnail : System.Web.UI.UserControl
   {
      private const String PhotoKey = "PHKey";

      [Browsable(true)]
      public event EventHandler DataChanged;

      protected virtual void OnDataChanged()
      {
         EventHandler temp = DataChanged;
         if (temp != null) 
            DataChanged(this, EventArgs.Empty);
      }

      [Bindable(true, BindingDirection.TwoWay)]
      public Model.Photo Photo
      {
         get
         {
            Model.Photo photo = (Model.Photo)ViewState[PhotoKey];
            if (photo != null)
               SyncDomainObject(photo);
            return photo;
         }
         set
         {
            ViewState[PhotoKey] = value;
            SyncInterface(value);
         }
      }

      private void SyncDomainObject(Model.Photo photo)
      {

      }

      private void SyncInterface(Model.Photo photo)
      {

         imgThumb.ImageUrl = PhotoLoader.GenerateLinkForPhoto(photo.ThumbNailFileName);
         imgThumb.AlternateText = photo.OriginalFileName + " " + photo.Description;
         lblDescription.Text = photo.Description ?? "No description";
      }

      /// <summary>
      /// depending on txtEdit visibility we are in edit or not
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      protected void btnEdit_Click(object sender, ImageClickEventArgs e)
      {
         if (!txtEdit.Visible)
         {
            txtEdit.Visible = true;
            lblDescription.Visible = false;
            txtEdit.Text = lblDescription.Text;
         }
         else
         {
            txtEdit.Visible = false;
            lblDescription.Visible = true;
            if (Services.PhotoManagerService.ChangePhotoDescription(Photo.Id, txtEdit.Text))
               lblDescription.Text = txtEdit.Text;
         }

      }

      protected void btnMoveBack_Click(object sender, ImageClickEventArgs e)
      {
         if (Services.PhotoManagerService.MovePhotoBack(Photo.Id))
            OnDataChanged();
      }

      protected void btnMoveForward_Click(object sender, ImageClickEventArgs e)
      {
         if (Services.PhotoManagerService.MovePhotoForward(Photo.Id))
            OnDataChanged();
      }
   }
}