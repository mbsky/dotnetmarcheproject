<%@ Page Language="C#"
    MasterPageFile="~/Views/Shared/Mvc.Master" 
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="maincontentid" ContentPlaceHolderID="maincontent" runat="server">
<h1><%=Html.Encode(ViewData["Message"]) %></h1>
</asp:Content>