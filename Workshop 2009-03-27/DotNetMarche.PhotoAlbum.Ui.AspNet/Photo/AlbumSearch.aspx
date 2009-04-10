<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AlbumSearch.aspx.cs" Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.AlbumSearch" %>
<%@ Register src="Controls/AlbumSearch.ascx" tagname="AlbumSearch" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <uc1:AlbumSearch ID="AlbumSearch1" runat="server" />

</asp:Content>
