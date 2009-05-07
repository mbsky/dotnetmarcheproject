using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcHelper;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic.Data;


namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
   public class MvcMasterController : Controller
   {
      private MasterLogic masterLogic;
      protected MasterLogic MasterLogic
      {
         get
         {
            return masterLogic ?? (masterLogic = new MasterLogic(new MvcRouteHelper(Url)));
         }
      }
      
      /// <summary>
      /// Key is text, and value is the url pointing to.
      /// </summary>
      public MenuItem RootMenu
      {
         get { return rootMenu ?? (rootMenu = MasterLogic.CreateMenu(Path.Combine(Global.PhysicalPath, "WebMvcSitemap.Xml"))); }
      }
      private MenuItem rootMenu;

   }
}
