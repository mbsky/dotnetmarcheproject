<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
   CodeBehind="AlbumSearchPr.aspx.cs" Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.AlbumSearchPr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
      <Scripts>
         <asp:ScriptReference Path="~/JQuery/AlbumSearchPr.js" />
      </Scripts>
      <Services>
      </Services>
   </asp:ScriptManagerProxy>
   <div id="gridlocation" class="editdiv">
   </div>
</asp:Content>
