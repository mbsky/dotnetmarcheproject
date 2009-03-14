<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
   CodeBehind="PhotoAlbumManager.aspx.cs" Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.PhotoAlbumManager" %>

<%@ Register Src="Controls/PhotoAlbumManager.ascx" TagName="PhotoAlbumManager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <title>Photo Management</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
      <Scripts>
         <asp:ScriptReference Path="~/JQuery/PhotoAlbumManager.js" />
      </Scripts>
      <Services>
      <asp:ServiceReference Path="~/Services/PhotoManager.asmx" />
      </Services>
   </asp:ScriptManagerProxy>
   <%--Control to manage photo Album--%>
   <uc1:PhotoAlbumManager ID="PhotoAlbumManager1" runat="server" />
</asp:Content>
