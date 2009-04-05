using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.IO;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models.Data;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
   public class MvcMasterController : Controller
   {
      private MasterModel masterModel;
      protected MasterModel MasterModel
      {
         get
         {
            return masterModel ?? (masterModel = new MasterModel());
         }
      }
      
      /// <summary>
      /// Key is text, and value is the url pointing to.
      /// </summary>
      public MenuItem RootMenu
      {
         get { return rootMenu ?? (rootMenu = MasterModel.CreateMenu(Path.Combine(Global.PhysicalPath,"WebMvcSitemap.Xml"))); }
      }
      private MenuItem rootMenu;

   }
}
