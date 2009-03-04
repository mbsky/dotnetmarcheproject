<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PhotoAlbumManager.aspx.cs" Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.PhotoAlbumManager" %>
<%@ Register src="Controls/PhotoAlbumManager.ascx" tagname="PhotoAlbumManager" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <title>Photo Management</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%--Control to manage photo Album--%>
   <uc1:PhotoAlbumManager ID="PhotoAlbumManager1" runat="server" />
   
</asp:Content>
