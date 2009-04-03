<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Xml.Linq" %> 

 <ul>
<%
foreach (XElement element in ((XElement) ViewData["Menu"]).Descendants()) { 
   if (element.Attribute("url") != null) {
%>

  <li><a href=" <%= Html.Encode(element.Attribute("url").Value)%>">
   <%= Html.Encode(element.Attribute("title").Value)%>
  </a></li> 
 
<%
   }
  }
%>
 </ul>