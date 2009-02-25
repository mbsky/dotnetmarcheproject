using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DotNetMarche.PhotoAlbum.Model.PhotoRoutines
{
   public class PhotoSizeManager
   {
      public Int32 MaxWidth { get; set; }
      public Int32 MaxHeight { get; set; }

      public PhotoSizeManager(int maxWidth, int maxHeight)
      {
         MaxWidth = maxWidth;
         MaxHeight = maxHeight;
      }

      /// <summary>
      /// Given a size it returns a resized size keeping proportion unchanged
      /// </summary>
      /// <param name="original"></param>
      /// <returns></returns>
      public Size Resize(Size original)
      {
         if (original.Width > MaxWidth || original.Height > MaxHeight)
         {
            //Needs resize
            Double xRatio = (Double) MaxWidth / original.Width;
            Double yRatio = (Double) MaxHeight / original.Height;
            //Now check with is the smaller one.
            if (xRatio < yRatio)
               return new Size(MaxWidth, (Int32) Math.Round(original.Height * xRatio));
            else
               return new Size((Int32)Math.Round(original.Width * yRatio), MaxHeight);
         }
         return original;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="fileName"></param>
      /// <returns></returns>
      public Bitmap ResizeBitmap(String fileName)
      {
         using (Bitmap b = new Bitmap(fileName))
         {
            Size resize = Resize(b.Size);
            Bitmap resized = new Bitmap(resize.Width, resize.Height, b.PixelFormat);
            using (Graphics g = Graphics.FromImage(resized))
            {
               g.InterpolationMode = InterpolationMode.HighQualityBicubic;
              g.DrawImage(b, 0, 0, resize.Width, resize.Height);
            }
            return resized;
         }
      }
   }
}