﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlbumViewer.ascx.cs"
   Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls.AlbumViewer" %>
<asp:ObjectDataSource ID="odsPhotoAlbum" runat="server" OldValuesParameterFormatString="original_{0}"
   SelectMethod="GetPhotoForAlbum" TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum"
   OnSelected="odsPhotoAlbum_Selected">
   <SelectParameters>
      <asp:QueryStringParameter DbType="Guid" Name="albumId" QueryStringField="albumId" />
   </SelectParameters>
</asp:ObjectDataSource>
<div id="albumviewer">
   <div id="maincontent">
      <asp:Image ID="imgMain" runat="server" />
   </div>
   <div id="thumbstrip">
      <asp:Repeater ID="Repeater1" runat="server" DataSourceID="odsPhotoAlbum">
         <ItemTemplate>
            <div id="photoselector">
               <asp:ImageButton ID="ImageButton1" runat="server" CssClass="thumbimage" photoname='<%# GeneratePhotoUrl(Eval("FileName")) %>'
                  OnClick="ChangePhoto" ImageUrl='<%# GeneratePhotoUrl(Eval("ThumbnailFileName")) %>' />
            </div>
         </ItemTemplate>
      </asp:Repeater>
   </div>
</div>