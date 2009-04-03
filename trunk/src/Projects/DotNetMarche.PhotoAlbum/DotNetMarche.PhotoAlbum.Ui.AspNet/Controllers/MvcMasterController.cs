using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
   public class MvcMasterController : Controller
   {
      private XElement _menuData;

      /// <summary>
      /// Key is text, and value is the url pointing to.
      /// </summary>
      public XElement MenuData
      {
         get { return _menuData; }
      }

      public MvcMasterController()
      {
         XDocument doc = XDocument.Load(Path.Combine(Global.PhysicalPath, "WebMvc.sitemap"));
         XNamespace ns =  XNamespace.Get("http://schemas.microsoft.com/AspNet/SiteMap-File-1.0");
         _menuData = doc.Root.Element(ns +"siteMapNode");
         ViewData["Menu"] = _menuData;
      }
   }
}
