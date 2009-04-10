using DotNetMarche.PhotoAlbum.Model;

namespace DotNetMarche.PhotoAlbum.Service.ContextManagement
{
   /// <summary>
   /// Interface for managing the context.
   /// </summary>
   interface IContextManager
   {
      /// <summary>
      /// Get the current context.
      /// </summary>
      /// <returns></returns>
      PhotoAlbumEntities GetCurrent();
   }
}