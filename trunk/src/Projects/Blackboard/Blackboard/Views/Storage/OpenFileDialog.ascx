<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="filelist" class="filelist ui-corner-all">
    
</div>
<textarea id="tplFileEnty" class="template">
    {#foreach $T as record}
        <div class="fileentry ui-corner-all">
            <span class="fileName">{$T.record.Name}</span> 
            size <span class="fileSize">{$T.record.Bytes}</span> 
            modified <span class="fileLastModified">{$T.record.LastModified}</span> 
        </div>
    {#/for}
</textarea>
