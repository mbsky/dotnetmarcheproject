<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AlbumViewer.aspx.cs" Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.AlbumViewer" %>
<%@ Register src="Controls/AlbumViewer.ascx" tagname="AlbumViewer" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
      <Scripts>
         <asp:ScriptReference Path="~/JQuery/PhotoAlbumViewer.js" />
      </Scripts>
      <Services>
         <asp:ServiceReference Path="~/Services/PhotoManager.asmx" />
      </Services>
   </asp:ScriptManagerProxy>
   
   
   <uc1:AlbumViewer ID="AlbumViewer1" runat="server" />
</asp:Content>
