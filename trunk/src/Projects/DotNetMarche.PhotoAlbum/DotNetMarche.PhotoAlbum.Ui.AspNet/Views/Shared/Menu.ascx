<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Master>" %>
<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>
<%@ Import Namespace="System.Xml.Linq" %>
<ul>
   <%
      foreach (DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic.Data.MenuItem item in ViewData.Model.MainMenu.MenuItems)
      { 
   %>
   <li><%= item.Render()%>
   <% if (item.MenuItems.Count > 0)
      {%>

      <ul>
         <%
            foreach (DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic.Data.MenuItem inneritem in item.MenuItems)
            {
         %>
         <li>
            <%=inneritem.Render()%>
         </li>
         <%
            }
         %>
      </ul>
   </li>
   <%
      }
      }
   %>
</ul>
