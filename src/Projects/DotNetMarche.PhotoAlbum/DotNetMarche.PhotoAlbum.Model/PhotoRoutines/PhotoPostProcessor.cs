using System;
using System.Drawing;
using DotNetMarche.Utils;

namespace DotNetMarche.PhotoAlbum.Model.PhotoRoutines
{
   public static class PhotoPostProcessor
   {
      static PhotoSizeManager resizeManager = new PhotoSizeManager(
            Properties.Settings.Default.MaxPhotoWidth,
            Properties.Settings.Default.MaxPhotoHeight);
      static PhotoSizeManager thumbNailManager = new PhotoSizeManager(
        Properties.Settings.Default.ThumbSize,
        Properties.Settings.Default.ThumbSize);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="fileName"></param>
      public static Photo ProcessPhoto(String fileName)
      {
         //First of all resize the original image and create thumbnail
         using (Bitmap resized = resizeManager.ResizeBitmap(fileName))
         using (Bitmap thumbNail = thumbNailManager.ResizeBitmap(fileName))
         {
            String FileName = PhotoFileManager.GenerateName();
            String ThumbFileName = GetThumbnailImageNameFromOriginalImageName(FileName);
            PhotoFileManager.SaveImage(resized, FileName);
            PhotoFileManager.SaveImage(thumbNail, ThumbFileName);
            return new Photo()
                      {
                         Id = GenericUtils.CreateSequentialGuid(),
                         FileName = FileName,
                         OriginalFileName = FileName,
                         ThumbNailFileName = ThumbFileName,
                         UploadDate = DateTime.Now,
                      };
         }
      }

      internal static String GetThumbnailImageNameFromOriginalImageName(string FileName)
      {
         return FileName.Replace(".jpg", "_T.jpg");
      }
   }
}
