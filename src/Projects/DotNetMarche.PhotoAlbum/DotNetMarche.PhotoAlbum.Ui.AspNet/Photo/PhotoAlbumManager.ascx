<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoAlbumManager.ascx.cs" Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.PhotoAlbumManager" %>
<%--<span class="baseSpan">Select a file to upload</span>
<asp:FileUpload ID="FileUpload1" runat="server" />--%>

<%@ Register Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.Parameters" TagPrefix="params" %>
<asp:ObjectDataSource ID="odsPhotoAlbum"
   runat="server" 
   DataObjectTypeName="DotNetMarche.PhotoAlbum.Model.PhotoAlbum" 
   InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
   SelectMethod="GetAll" 
   TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum" 
   UpdateMethod="Update">
   <SelectParameters>
      <params:CurrentUserIdParameter DbType="Guid" Name="userId"  />
   </SelectParameters>
</asp:ObjectDataSource>

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
   DataSourceID="odsPhotoAlbum">
   <Columns>
      <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
      <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
      <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
      <asp:BoundField DataField="Description" HeaderText="Description" 
         SortExpression="Description" />
      <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" 
         SortExpression="CreationDate" />
      <asp:BoundField DataField="Status" HeaderText="Status" 
         SortExpression="Status" />
   </Columns>
</asp:GridView>
