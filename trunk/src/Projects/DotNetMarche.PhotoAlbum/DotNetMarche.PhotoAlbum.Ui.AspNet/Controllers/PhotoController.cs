using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers.ViewData;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
    public class PhotoController : MvcMasterController
    {

        public ActionResult ManageAlbum(Int32? id, Int32? pageid)
        {
           AlbumManagerViewData model = new AlbumManagerViewData();
           model.MainMenu = base.RootMenu;
         
            ViewData["test"] = "This is page that shows the user album:" + id + " " + pageid;
            return View();
        }
    }


}
