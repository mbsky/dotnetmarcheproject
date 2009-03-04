<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhotoAlbumManager.ascx.cs"
   Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls.PhotoAlbumManager" %>
<%--<span class="baseSpan">Select a file to upload</span>
<asp:FileUpload ID="FileUpload1" runat="server" />--%>
<%@ Register 
   Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.Parameters"
   Assembly="DotNetMarche.PhotoAlbum.Ui.AspNet"
   TagPrefix="cc1" %>

<asp:ObjectDataSource ID="odsPhotoAlbum" runat="server" DataObjectTypeName="DotNetMarche.PhotoAlbum.Model.PhotoAlbum"
   InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll"
   TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum" UpdateMethod="Update">
   <SelectParameters>
      <cc1:CurrentUserIdParameter DbType="Guid" Name="userId" />
   </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="odsPhoto" runat="server" OldValuesParameterFormatString="original_{0}"
   SelectMethod="GetPhotoForAlbum" 
   TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum">
   <SelectParameters>
      <asp:ControlParameter ControlID="grdPhotoAlbum" DbType="Guid" Name="albumId" PropertyName="SelectedValue" />
   </SelectParameters>
</asp:ObjectDataSource>

<div id="listOfPhotoAlbum">
   <asp:GridView ID="grdPhotoAlbum" runat="server" AutoGenerateColumns="False" DataSourceID="odsPhotoAlbum"
      DataKeyNames="Id" style="margin-top: 0px">
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
   <asp:Repeater ID="Repeater1" runat="server" DataSourceID="odsPhoto">
      <ItemTemplate>
         <div id='<%# "photo_id" + Eval("Id") %>'>
            <img src='<%# "photo/" + Eval("ThumbNailFileName") + ".jpg" %>' alt='<%# Eval("OriginalFileName") + " " + Eval("Description") %>' />
         </div>
      </ItemTemplate>
      <FooterTemplate>
         <div id="addNewPhotoToAlbum">
            <span>Add new photo to album</span>
            <asp:FileUpload ID="FileUpload1" runat="server" />
         </div>
      </FooterTemplate>
   </asp:Repeater>
</div>
