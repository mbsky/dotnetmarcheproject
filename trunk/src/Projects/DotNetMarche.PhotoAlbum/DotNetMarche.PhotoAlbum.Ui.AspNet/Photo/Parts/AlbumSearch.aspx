<%@ Page EnableViewState="false" Language="C#" AutoEventWireup="true" CodeBehind="AlbumSearch.aspx.cs"
   Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Parts.AlbumSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
</head>
<body>
   <form id="form1" runat="server">
   <div id="thecontent">
      <asp:ObjectDataSource EnableViewState="false" ID="odsAlbumSearch" runat="server" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
         SelectMethod="SearchAlbum" SortParameterName="sortClause" TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum"
         SelectCountMethod="SearchAlbumGetCount" OnSelecting="odsAlbumSearch_Selecting">
         <SelectParameters>
            <asp:QueryStringParameter Name="name" QueryStringField="name" Type="String" />
            <asp:QueryStringParameter Name="description" QueryStringField="desc" Type="String" />
            <asp:QueryStringParameter Name="user" QueryStringField="user" Type="String" />
         </SelectParameters>
      </asp:ObjectDataSource>
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
      <asp:GridView EnableViewState="false" ID="grdAlbum" runat="server" AllowPaging="True" AutoGenerateColumns="False"
         DataSourceID="odsAlbumSearch" AllowSorting="True" CssClass="baseTable" DataKeyNames="Id"
         PagerStyle="pagerstyle" HeaderStyle="headerstyle">
         <Columns>
            <%-- <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />--%>
            <asp:TemplateField>
               <ItemTemplate>
                  <a href='<%# CreateAlbumLink(Eval("Id")) %>'>Visualizza</a>
               </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="Users.UserName" />
            <%--      <asp:BoundField DataField="UserId" HeaderText="UserId" 
         SortExpression="UserId" />--%>
         </Columns>
         <PagerStyle CssClass="pagerStyle" />
         <HeaderStyle CssClass="headerStyle" />
      </asp:GridView>
   </div>
   </form>
</body>
</html>
