using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers.Interfaces
{
   /// <summary>
   /// To permit test when controllers or models are using cache
   /// </summary>
   public interface ICache
   {
      Object this[String key] { get; }



   }
}