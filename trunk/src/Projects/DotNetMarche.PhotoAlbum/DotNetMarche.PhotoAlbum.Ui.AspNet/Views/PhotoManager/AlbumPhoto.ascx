<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AlbumManager>" %>
<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>
<%@ Import Namespace="System.Xml.Linq" %>

   <%
      foreach (DotNetMarche.PhotoAlbum.Model.Photo photo in ViewData.Model.PhotoForCurrentAlbum)
      { 
   %>
   <div id="photo_<%= photo.Id %>" class="thumbnail">
    
      <img alt="<%= photo.Description %>" src="/Photo/<%=photo.ThumbNailFileName%>.axd" />
      <span><%= photo.Description %></span>
      
   </div>
   <% } %>
