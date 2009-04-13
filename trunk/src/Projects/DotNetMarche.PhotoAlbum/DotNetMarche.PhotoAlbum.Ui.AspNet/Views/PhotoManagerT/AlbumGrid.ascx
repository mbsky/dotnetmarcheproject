<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AlbumManager>" %>
<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>
<%@ Import Namespace="System.Xml.Linq" %>
<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic" %>

<table class="baseTable" border="1" cellspacing="0"><tbody>
<tr><th></th><th></th><th>Name</th><th>Description</th><th>Creation Date</th></tr>
   <%
      foreach (DotNetMarche.PhotoAlbum.Model.PhotoAlbum album in ViewData.Model.Albums)
      { 
   %>
   <tr>
    
    <td><%= Html.RouteLink("Edit", "PagedController", new
                                                                     {
                                                                        controller = "PhotoManager",
                                                                        action = "ManageAlbum", 
                                                                        pageid = ViewData.Model.CurrentPage, 
                                                                        id = album.Id,
                                                                     }, null)%></td>  

    <td><%= Url.PartialRenderingRouteLink("photolist", "EditP", "Default", new 
                                                                     {
                                                                        controller = "PhotoManager",
                                                                        action = "GenerateEditAlbum", 
                                                                        albumId = album.Id,
                                                                     })%></td>  
      <td><%= album.Name %></td>
      <td><%= album.Description %></td>
      <td><%= album.CreationDate.ToShortDateString() %></td>
   </tr>
   <% } %>
</tbody></table> 