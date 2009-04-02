using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Blackboard.Models.Storage;

namespace Blackboard.Controllers
{
    public class StorageController : Controller
    {
        public const string StorageFolder = "~/Content/Storage/";

        public ActionResult List()
        {
            var listOfFiles = Directory.GetFiles(Server.MapPath(StorageFolder), "*.blackboard", SearchOption.TopDirectoryOnly);
            var result = new List<FileEntry>();

            foreach (var file in listOfFiles)
                result.Add(new FileEntry(new FileInfo(file)));

            return Json(result);
        }

        public ActionResult OpenFileDialog()
        {
            return PartialView("OpenFileDialog");
        }

        public ActionResult OpenImageDialog()
        {
            return PartialView("OpenImageDialog");
        }

        public ActionResult ListImages()
        {
            var basePath = Server.MapPath("~");
            var imgList = from s in
                              Directory.GetFiles(Server.MapPath("~/Content/Libs/img/"), "*.png")
                          select new
                          {
                              src = s.Remove(0, basePath.Length - 1).Replace("\\", "/"),
                              name = Path.GetFileNameWithoutExtension(s)
                          };

            return Json(imgList);
        }

        public ContentResult GetFile(string name)
        {
            var filename = Path.Combine(Server.MapPath(StorageFolder), name);
            return Content(System.IO.File.ReadAllText(filename));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(string name, string author, string slidesource)
        {
            object result = null;
            try
            {
                if (!name.EndsWith(".blackboard"))
                    name = name + ".blackboard";

                var filename = Path.Combine(Server.MapPath(StorageFolder), name);

                System.IO.File.WriteAllText(filename, slidesource);

                FileInfo fi = new FileInfo(filename);

                result = new { status = "ok", filesize = fi.Length };

            }
            catch (Exception ex)
            {
                result = new { status = "fail", message = ex.Message };
            }

            return Json(result);
        }
    }
}
