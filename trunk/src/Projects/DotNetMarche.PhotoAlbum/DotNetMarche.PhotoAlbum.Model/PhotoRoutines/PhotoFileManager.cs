using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DotNetMarche.PhotoAlbum.Model.PhotoRoutines
{
   /// <summary>
   /// Generates names from the photo.
   /// </summary>
   public static class PhotoFileManager
   {

      public static Func<String> NameRandomGenerator = () =>
       {
          RNGCryptoServiceProvider provider =
             new RNGCryptoServiceProvider();
          Byte[] name = new byte[32];
          provider.GetBytes(name);
          return BitConverter.ToString(name).Replace("-", "");
       };

      public static DisposableAction OverrideGenerator(Func<String> newGenerator)
      {
         Func<String> current = NameRandomGenerator;
         NameRandomGenerator = newGenerator;
         return new DisposableAction(() => NameRandomGenerator = current  );
      }

      public static string GenerateName()
      {
         String randomFileName = null;
         while (String.IsNullOrEmpty(randomFileName) || File.Exists(randomFileName))
         {
            
            randomFileName = Path.ChangeExtension(
               Path.Combine(
                  Properties.Settings.Default.PhisicalPhotoPath,
                  NameRandomGenerator()),
               "jpg");
         }

         return randomFileName;
      }

      public static String SaveImage(Bitmap image)
      {
         return SaveImage(image, GenerateName());
      }

      public static string SaveImage(Bitmap image, String fileName)
      {
         if (!Directory.Exists(Properties.Settings.Default.PhisicalPhotoPath))
         {
            Directory.CreateDirectory(Properties.Settings.Default.PhisicalPhotoPath);
         }
         image.Save(fileName, ImageFormat.Jpeg);
         return fileName;
      }
   }
}
