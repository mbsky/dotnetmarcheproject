using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blackboard.Services;

namespace Blackboard
{
    public class ThumbnailResult : FileResult
    {
        private readonly string _sourceFileName;
        private string _thumbFileName;

        public ThumbnailResult(string sourceFileName)
            :base(GetContentTypeFromFileName(sourceFileName))
        {
            _sourceFileName = sourceFileName;
        }

        private static string GetContentTypeFromFileName(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLowerInvariant())
            {
                case ".png":
                    return "image/png";
                
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
             }

            return "image/png";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string fname = context.HttpContext.Server.MapPath(_sourceFileName);
            var service = new ThumbnailService();
            _thumbFileName = service.GetOrCreateThumb(fname);

            base.ExecuteResult(context);
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.TransmitFile(this._thumbFileName);   
        }
    }
}
