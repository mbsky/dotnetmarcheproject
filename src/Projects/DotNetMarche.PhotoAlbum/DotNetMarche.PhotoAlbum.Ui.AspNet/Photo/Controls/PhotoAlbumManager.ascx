<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoAlbumManager.ascx.cs"
   Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls.PhotoAlbumManager" %>
<%@ Register Src="SinglePhotoThumbnail.ascx" TagName="SinglePhotoThumbnail" TagPrefix="uc1" %>
<%@ Register Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.Parameters"
   Assembly="DotNetMarche.PhotoAlbum.Ui.AspNet" TagPrefix="cc1" %>
<asp:ObjectDataSource ID="odsPhotoAlbum" runat="server" DataObjectTypeName="DotNetMarche.PhotoAlbum.Model.PhotoAlbum"
   InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll"
   TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum" UpdateMethod="Update">
   <SelectParameters>
      <cc1:CurrentUserIdParameter DbType="Guid" Name="userId" />
   </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsPhoto" runat="server" OldValuesParameterFormatString="original_{0}"
   SelectMethod="GetAlbumWithPhoto" TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum">
   <SelectParameters>
      <asp:ControlParameter ControlID="grdPhotoAlbum" DbType="Guid" Name="albumId" PropertyName="SelectedValue"
         DefaultValue="" />
   </SelectParameters>
</asp:ObjectDataSource>
<div id="listOfPhotoAlbum">
   <asp:GridView ID="grdPhotoAlbum" runat="server" AutoGenerateColumns="False" DataSourceID="odsPhotoAlbum"
      DataKeyNames="Id" Style="margin-top: 0px">
      <Columns>
         <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
         <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
         <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
         <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" SortExpression="CreationDate"
            ReadOnly="True" />
         <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ReadOnly="True"
            Visible="False" />
      </Columns>
      <EmptyDataTemplate>
         <span class="warn">There is no album defined, please define one.</span>
      </EmptyDataTemplate>
   </asp:GridView>
</div>
<div id="insertNewPhotoAlbum">
   <asp:Label ID="Label1" runat="server" Text="Name"></asp:Label>
   <asp:TextBox ID="txtNameForNewElement" runat="server"></asp:TextBox>
   <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
   <asp:TextBox ID="txtdescriptionForNewElement" runat="server" TextMode="MultiLine"
      Height="57px"></asp:TextBox>
   <asp:Button ID="btnAddNewElement" runat="server" Text="AddNew" OnClick="btnAddNewElement_Click"
      Width="70px" />
</div>
<div id="editPhotoAlbum">
   <asp:FormView ID="frmEdit" runat="server" DataSourceID="odsPhoto">
      <ItemTemplate>
         <div id="editAlbumData">
            <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
         </div>
         <div id="photolist">
            <asp:Repeater ID="rptPhoto" runat="server" DataSource='<%# Eval("Photo") %>'>
               <ItemTemplate>
                  <uc1:SinglePhotoThumbnail ID="SinglePhotoThumbnail1" runat="server" sdfgsdfgsdfgs="SDFGSDFGS"
                     Photo='<%# Container.DataItem %>' OnDataChanged="SinglePhoto_DataChanged" />
               </ItemTemplate>
            </asp:Repeater>
         </div>
         <asp:Label ID="Label3" runat="server" Text='Aggiungi foto'></asp:Label>
         <div id="fileUpload">
            <asp:FileUpload ID="upPhoto" runat="server" />
            <asp:Button ID="btnUploadPhoto" runat="server" CommandArgument='<%# Eval("Id") %>'
               Text="Upload" OnClick="btnUploadPhoto_Click" />
         </div>
      </ItemTemplate>
   </asp:FormView>
</div>
