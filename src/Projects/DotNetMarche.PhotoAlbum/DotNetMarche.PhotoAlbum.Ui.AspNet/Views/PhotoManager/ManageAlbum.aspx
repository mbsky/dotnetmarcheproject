<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mvc.Master" Inherits="System.Web.Mvc.ViewPage<AlbumManager>"
   Theme="Mvc" %>

<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">

   <script src="/JQuery/PhotoAlbumManagerMvc.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="maincontentid" ContentPlaceHolderID="maincontent" runat="server">

   <div id="listOfPhotoAlbum" class="editdiv">
      <% Html.RenderPartial("AlbumGrid"); %>
   </div>
   <div id="photolist">
      <% Html.RenderPartial("AlbumPhoto"); %>
   </div>
</asp:Content>
