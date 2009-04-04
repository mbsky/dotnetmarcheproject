<%@ Page Language="C#"
    MasterPageFile="~/Views/Shared/Mvc.Master" 
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="maincontentid" ContentPlaceHolderID="maincontent" runat="server">
    <h1>This is the photoalbum:<%=Html.Encode(ViewData["test"])%></h1>
</asp:Content>