<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SinglePhotoThumbnail.ascx.cs"
   Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls.SinglePhotoThumbnail" %>
<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Handler"%>
<div id='<%= "photo_" + Photo.Id %>' class="thumbnail">
      <img src="<%# PhotoLoader.GenerateLinkForPhoto(Photo.ThumbNailFileName) %>"
       alt="<%= Photo.OriginalFileName + " " + Photo.Description %>" />
      <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
      <asp:TextBox ID="txtEdit" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
      <asp:ImageButton ID="btnMoveBack" runat="server"  
         ImageUrl="~/images/MoveBack.png" AlternateText="Sposta l'immagine a sinistra" 
         onclick="btnMoveBack_Click"/>
      <asp:ImageButton ID="btnEdit" runat="server"  
         ImageUrl="~/images/EditComment.png" AlternateText="Edita il commento" 
         onclick="btnEdit_Click" />
      <asp:ImageButton ID="btnMoveForward" runat="server"  
         ImageUrl="~/images/MoveForward.png" AlternateText="Sposta l'immagine a destra" 
         onclick="btnMoveForward_Click"/>
</div>
