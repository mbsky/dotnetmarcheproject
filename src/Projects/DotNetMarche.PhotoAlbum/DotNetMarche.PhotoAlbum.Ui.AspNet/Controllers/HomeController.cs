using System.Web.Mvc;

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