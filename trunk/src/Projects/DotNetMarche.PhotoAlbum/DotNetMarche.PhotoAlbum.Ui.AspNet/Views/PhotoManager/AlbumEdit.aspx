<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<AlbumManager>" Theme="" %>

<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>

   <div id="thecontent">
      <% Html.RenderPartial("AlbumPhoto"); %>
   </div>

