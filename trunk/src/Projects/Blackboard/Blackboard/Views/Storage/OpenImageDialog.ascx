<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<string>>" %>
<div id="imgList" class="imgList ui-corner-all">
    
</div>
<div class="clearFix"></div>
<textarea id="tplImageEnty" class="template">
    {#foreach $T as record}
        <div class="imageentry ui-corner-all">
            <img src="{$T.record.src}" />
            <span>{$T.record.name}</span>
        </div>
    {#/for}
</textarea>
