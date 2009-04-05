using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Blackboard.Services
{
    public class ThumbnailService
    {
        public string StorageSubFolder { get; set;}
        public InterpolationMode InterpolationMode { get; set; }
        public Size DestinationSize { get; set; }
        public ImageFormat DestinationImageFormat { get; set; }
        public Color BackgroundColor { get; set; }
        public ThumbnailService()
        {
            this.StorageSubFolder = "thumbs";
            this.InterpolationMode = InterpolationMode.Bilinear;
            this.DestinationSize = new Size(64,64);
            this.DestinationImageFormat = ImageFormat.Png;
            this.BackgroundColor = Color.Empty;
        }

        public string GetOrCreateThumb(string fname)
        {
            string destName = MakeValidThumbnailFileName(fname);
            if (File.Exists(destName))
            {
                FileInfo sourceFileInfo = new FileInfo(fname);
                FileInfo destFileInfo = new FileInfo(destName);

                if (sourceFileInfo.LastWriteTime <= destFileInfo.LastWriteTime)
                    return destName;
            }


            Bitmap sourceBmp = new Bitmap(fname);
            Bitmap destBmp = new Bitmap(this.DestinationSize.Width, this.DestinationSize.Height);

            double resizeFactor = Math.Min(
                DestinationSize.Width / (double)sourceBmp.Size.Width,
                DestinationSize.Height / (double)sourceBmp.Size.Height
            );

            Size thumbInternalSize = new Size((int)(sourceBmp.Size.Width * resizeFactor),
                (int)(sourceBmp.Size.Height * resizeFactor));


            using (Graphics gr = Graphics.FromImage(destBmp))
            {
                if(this.BackgroundColor != Color.Empty)
                {
                   gr.Clear(this.BackgroundColor);   
                }

                gr.InterpolationMode = this.InterpolationMode;
                
                gr.DrawImage(sourceBmp,
                    new RectangleF(PointF.Empty, thumbInternalSize),
                    new RectangleF(PointF.Empty, sourceBmp.Size), 
                    GraphicsUnit.Pixel
               );
            }

            destBmp.Save(destName, this.DestinationImageFormat);

            return destName;
        }

        private string MakeValidThumbnailFileName(string fullSizeImageFileName)
        {
            string baseFolder = Path.Combine(Path.GetDirectoryName(fullSizeImageFileName), this.StorageSubFolder);

            if (!Directory.Exists(baseFolder))
                Directory.CreateDirectory(baseFolder);

            return Path.Combine(
                baseFolder,
                Path.GetFileName(fullSizeImageFileName)
                );
        }
    }
}
