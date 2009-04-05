using System.Web.Mvc;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers.ViewData;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Controllers
{
    public class HomeController : MvcMasterController
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "This is ASP.NET MVC!";
            return View();
        }
    }
}