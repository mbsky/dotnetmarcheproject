<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Mvc.Master" Inherits="System.Web.Mvc.ViewPage<AlbumManager>"
   Theme="Mvc" %>

<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">

    <script src="/JQuery/PhotoAlbumManagerMvc.js" type="text/javascript"></script>
    <script src="/JQuery/jquery-ui-1.7.1.custom.js" type="text/javascript"></script>
    <script src="/JQuery/jquery-jtemplates.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="maincontentid" ContentPlaceHolderID="maincontent" runat="server">

   <div id="listOfPhotoAlbum" class="editdiv">
      
   </div>
   <div id="photolist">
     
   </div>
</asp:Content>
