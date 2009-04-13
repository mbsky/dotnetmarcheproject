<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AlbumManager>" %>
<%@ Import Namespace="DotNetMarche.PhotoAlbum.Ui.AspNet.Models" %>
<%@ Import Namespace="System.Xml.Linq" %>
<textarea id="tplSinglePhoto" class="template">
    <div id="photo_{$T.record.Name}" class="thumbnail">
      <img alt="{$T.record.Description}" src="/Photo/{$T.record.ThumbNailFileName}.axd" />
      <span>{$T.record.Description}</span>
   </div>
</textarea>

   
