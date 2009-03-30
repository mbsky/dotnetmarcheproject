<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="jQuery.Extensions.Helpers" %>
<%@ Import Namespace="jQuery.Extensions.ui" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scriptPlaceHolder" runat="server">
    <script src="../../Scripts/editor/jquery.wysiwyg.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-json.js" type="text/javascript"></script>
    <link href="../../Scripts/editor/jquery.wysiwyg.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-blackboard.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-blackboard-dialogs.js" type="text/javascript"></script>
    <script src="../../Scripts/colorpicker/js/colorpicker.js" type="text/javascript"></script>
    <link href="../../Scripts/colorpicker/css/colorpicker.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var services = [];
        services['openfiledialog'] = '<%= Url.Action("OpenFileDialog", "Storage") %>';
        services['openimagedialog'] = '<%= Url.Action("OpenImageDialog", "Storage") %>';
        services['listimages'] = '<%= Url.Action("ListImages", "Storage") %>';
        services['listfiles'] = '<%= Url.Action("List", "Storage") %>';
        services['getfile'] = '<%= Url.Action("GetFile", "Storage") %>';

        var serverUrls = [];
        serverUrls['images'] = '<%= Url.Content("~/Content/libs/backgrounds/") %>';
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="clearFix">
    </div>
    <div id="leftcolumn">
        <div id="toolbar-pane">
            <% 
                Toolbar
                    .Create()
                    .SetCorners(Corners.CornersType.top)
                    .AddSet()
                    .AddIconButton("btnOpen", "Open", "folder-open")
                    .AddIconButton("btnSave", "Save", "disk")
                    .AddIconButton("btnDelete", "Delete", "trash")
                    .EndSet()
                    .AddToggle()
                    .AddButton("btnToggleLock", "Lock")
                    .EndSet()
                    .AddSet()
                    .AddIconSolo("btnAddSlide", "Add slide", "plus")
                    .AddIconSolo("btnRemoveSlide", "Remove slide", "minus")
                    .AddIconSolo("btnAddText", "Add text", "pencil")
                    .AddIconSolo("btnAddImage", "Add image", "image")
                    .AddIconSolo("btnDeleteSelected", "Delete selected", "trash")
                    .AddIconSolo("btnAlignLeft", "Align left", "arrowstop-1-w")
                    .EndSet()
                    .Render(Writer);
            %>
        </div>
        <div class="slide-viewer">
            <div class="slide">
            </div>
        </div>
        <div id="navigator-pane" class="slide-navigator ui-corner-bottom ui-helper-clearfix ui-widget-header">
            <span id="slidenumber">1</span>
            <%= Html.jQueryUI().LinkButton("btnPrev", "seek-prev")%>
            <div id="navigator">
            </div>
            <%= Html.jQueryUI().LinkButton("btnNext", "seek-next")%>
            <%= Html.jQueryUI().LinkButton("btnPlay", "play")%>
        </div>
    </div>
    <div style="display: none">
        <div id="saveDialog" title="Save presentation">
            <p id="validateTips">
                Please insert presentation name and author.</p>
            <form id="savePresentationForm" method="post" action="<%= Url.Action("Save","Storage") %>">
            <fieldset>
                <label for="name">
                    Filename</label>
                <input type="text" name="name" id="name" class="text ui-widget-content ui-corner-all required"
                    minlength="2" />
                <label for="author">
                    Author</label>
                <input type="text" name="author" id="author" value="" class="text ui-widget-content ui-corner-all" />
                <input type="hidden" name="slidesource" id="slidesource" value="" />
            </fieldset>
            </form>
        </div>
        <div id="propertiesDialog" title="Properties">
            <form id="editTextForm">
            <fieldset>
                <label for="spanPropertyText">
                    Text</label>
                <input type="text" name="spanPropertyText" id="spanPropertyText" class="text ui-widget-content ui-corner-all" />
                <%
                    var listOfFontSizes = from s in new[] { "10", "12", "14", "16", "18", "20", "24", "30", "36", "40","48" } select new SelectListItem() { Text = s };
                %>
                <%= Html.DropDownList("ddlFontSize", listOfFontSizes )%>
                <p id="textColorPicker">
                </p>
            </fieldset>
            </form>
        </div>
        <div id="saveConfirmDialog" title="Presentation saved">
            <p>
                <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;">
                </span>Your presentation have successfully saved into the remote folder.
            </p>
            <p>
                File size: <span id="savedFileSize">&nbsp;</span>.
            </p>
        </div>
    </div>
</asp:Content>
