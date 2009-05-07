using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcHelper;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
   /// <summary>
   /// this is the master contorller for template it manages templates for the main page.
   /// </summary>
   public class MasterTController : Controller
   {

      public JsonResult Header()
      {
         HeaderT sendData = new HeaderT();
         sendData.UserName = System.Web.HttpContext.Current.User.Identity.Name;
         return Json(sendData);
      }

      public ActionResult HeaderTemplate()
      {
         return View("HeaderTemplate");
      }

      public ActionResult MenuTemplate()
      {
         return View("MenuTemplate");
      }

      public JsonResult Menu()
      {
         MasterLogic mastermodel = new MasterLogic(new MvcRouteHelper(Url));
         return Json(mastermodel.CreateMenu(Path.Combine(Global.PhysicalPath,"WebMvcSitemap.Xml")));
      }
   }
}
