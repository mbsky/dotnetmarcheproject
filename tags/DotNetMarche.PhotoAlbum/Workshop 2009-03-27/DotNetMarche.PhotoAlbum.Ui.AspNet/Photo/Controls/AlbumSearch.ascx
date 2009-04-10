<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlbumSearch.ascx.cs"
   Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Controls.AlbumSearch" %>
<asp:ObjectDataSource ID="odsAlbumSearch" runat="server" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
   SelectMethod="SearchAlbum" SortParameterName="sortClause" 
   TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum" 
   DataObjectTypeName="DotNetMarche.PhotoAlbum.Dto.PhotoAlbumInfo" 
   SelectCountMethod="SearchAlbumGetCount">
   <SelectParameters>
      <asp:ControlParameter ControlID="txtName" Name="name" PropertyName="Text" Type="String" />
      <asp:ControlParameter ControlID="txtDescription" Name="description" PropertyName="Text"
         Type="String" />
      <asp:ControlParameter ControlID="txtUser" Name="user" PropertyName="Text" Type="String" />
   </SelectParameters>
</asp:ObjectDataSource>
<div id="editArea" class="editdiv">

<div id="albumSearchData">
   <div id="name" class="editbox">
      <asp:Label ID="Label1" runat="server" Text="Name"></asp:Label>
      <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
   </div>
   <div id="description" class="editbox">
      <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
      <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
   </div>
   <div id="user" class="editbox">
      <asp:Label ID="Label3" runat="server" Text="User"></asp:Label>
      <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
   </div>
   <asp:Button ID="btnSearch" runat="server" Text="Search" />
</div>
<asp:GridView ID="grdAlbum" runat="server" AllowPaging="True" AutoGenerateColumns="False"
   DataSourceID="odsAlbumSearch" AllowSorting="True" CssClass="baseTable" 
   DataKeyNames="Id">
   <Columns>
      <%-- <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />--%>
      <asp:TemplateField>
         <ItemTemplate>
            <a href='<%# CreateAlbumLink(Eval("Id")) %>'>Visualizza</a>
         </ItemTemplate>
      </asp:TemplateField>
      <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
      <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
      <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
      <%--      <asp:BoundField DataField="UserId" HeaderText="UserId" 
         SortExpression="UserId" />--%>
   </Columns>
</asp:GridView>
</div>
