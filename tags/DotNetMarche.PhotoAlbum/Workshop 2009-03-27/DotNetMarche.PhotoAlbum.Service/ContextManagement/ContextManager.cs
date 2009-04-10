using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Model;

namespace DotNetMarche.PhotoAlbum.Service.ContextManagement
{
   public static class ContextManager
   {
      private static  IContextManager instance;

      /// <summary>
      /// Todo: Resolve with inversion of control
      /// </summary>
      static ContextManager()
      {
         instance = new ContextPerRequestContextManager();
      }

      public static PhotoAlbumEntities GetCurrent()
      {
         return instance.GetCurrent();
      }
   }
}
