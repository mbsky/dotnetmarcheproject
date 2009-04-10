using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.PhotoAlbum.Model
{
   public struct DisposableAction : IDisposable
   {

      private Action mGuardFunction;

      public DisposableAction(Action guardFunction)
      {
         this.mGuardFunction = guardFunction;
      }

      /// <summary>
      /// This function remove the guard callback, is to be used with great attenction
      /// by the caller, but is a way to avoid disposable action to be called at the 
      /// end of the scope. Remember that inside a using block there is no good way
      /// to avoid Dispose to be called.
      /// </summary>
      internal void Dismiss()
      {
         mGuardFunction = null;
      }

      #region IDisposable Members

      public void Dispose()
      {
         if (mGuardFunction != null)
            mGuardFunction();
      }

      #endregion


   }
}
