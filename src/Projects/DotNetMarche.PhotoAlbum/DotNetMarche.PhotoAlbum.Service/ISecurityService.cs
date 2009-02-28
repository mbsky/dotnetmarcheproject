using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.PhotoAlbum.Service
{
   public interface ISecurityService
   {
      /// <summary>
      /// Created a user given an ID, the login system is separated
      /// from the user information, we assume that some login system
      /// is enabled to log in such a user, in this example it is ASP.NET
      /// membership.
      /// </summary>
      /// <param name="userId"></param>
      void CreateUser(Guid userId);
   }
}
