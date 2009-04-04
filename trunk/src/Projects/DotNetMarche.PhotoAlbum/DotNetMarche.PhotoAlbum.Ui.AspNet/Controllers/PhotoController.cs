using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
    public class PhotoController : MvcMasterController
    {
        public ActionResult ShowPhotoAlbum(Int32? id, Int32? pageid)
        {
            SetDataForMenu();
            ViewData["test"] = "This is a test:" + id + " " + pageid;
            return View();
        }
    }
}
