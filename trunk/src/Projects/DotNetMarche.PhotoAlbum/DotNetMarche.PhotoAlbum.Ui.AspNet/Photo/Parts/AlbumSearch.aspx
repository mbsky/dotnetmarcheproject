<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlbumSearch.aspx.cs" Inherits="DotNetMarche.PhotoAlbum.Ui.AspNet.Photo.Parts.AlbumSearch" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="thecontent">
      
<asp:ObjectDataSource ID="odsAlbumSearch" runat="server" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
   SelectMethod="SearchAlbum" SortParameterName="sortClause" 
   TypeName="DotNetMarche.PhotoAlbum.Ui.AspNet.DataSources.PhotoAlbum" 
   SelectCountMethod="SearchAlbumGetCount">
   <SelectParameters>
      <asp:QueryStringParameter Name="name" QueryStringField="name" Type="String" />
      <asp:QueryStringParameter Name="description" QueryStringField="desc" 
         Type="String" />
      <asp:QueryStringParameter Name="user" QueryStringField="user" Type="String" />
   </SelectParameters>
</asp:ObjectDataSource>
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
    </form>
</body>
</html>
