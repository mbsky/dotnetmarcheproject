<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MvcT.Master" Inherits="System.Web.Mvc.ViewPage<AlbumManager>"
   Theme="Mvc" %>

<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>
   <asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>

<asp:Content ID="maincontentid" ContentPlaceHolderID="maincontent" runat="server">
   <div id="listOfPhotoAlbum" class="editdiv">
   </div>
   <div id="photolist">
   </div>
</asp:Content>
