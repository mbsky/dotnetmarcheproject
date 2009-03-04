using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using DotNetMarche.PhotoAlbum.Model;

namespace DotNetMarche.PhotoAlbum.Service.ContextManagement
{
   public class ContextPerRequestContextManager : IContextManager, IHttpModule
   {
      #region IContextManager Members

      public PhotoAlbumEntities GetCurrent()
      {
         if (HttpContext.Current.Items[httpContextKey] == null)
         {
            PhotoAlbumEntities context = new PhotoAlbumEntities();
            context.Connection.Open();
            HttpContext.Current.Items.Add(httpContextTransactionKey, context.Connection.BeginTransaction());
            HttpContext.Current.Items.Add(httpContextKey, context);
         }
         return (PhotoAlbumEntities)HttpContext.Current.Items[httpContextKey];
      }

      #endregion

      #region IHttpModule Members

      private const String httpContextKey = "PhotoAlbumEntities";
      private const String rollbackRequestedKey = "RollbackContextRequest";
      private const String httpContextTransactionKey = "PhotoAlbumEntitiesTransaction";

      public void Dispose()
      {

      }

      public void Init(HttpApplication context)
      {
         context.EndRequest += new EventHandler(context_EndRequest);
         context.Error += new EventHandler(context_Error);
      }

      /// <summary>
      /// There is an exception, rollback everything
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void context_Error(object sender, EventArgs e)
      {
         HttpContext.Current.Items.Add(rollbackRequestedKey, true);
      }

      void context_EndRequest(object sender, EventArgs e)
      {
         if (HttpContext.Current.Items[httpContextKey] != null)
         {
            using (PhotoAlbumEntities context = (PhotoAlbumEntities)HttpContext.Current.Items[httpContextKey])
            {
               Boolean RollbackRequested = (Boolean)(HttpContext.Current.Items[rollbackRequestedKey] ?? false);
               using (DbTransaction transaction = (DbTransaction)HttpContext.Current.Items[httpContextTransactionKey])
               {
                  if (RollbackRequested)
                  {
                     transaction.Rollback();
                  }
                  else
                  {
                     context.SaveChanges();
                     transaction.Commit();
                  }
               }
            }
         }
      }

      #endregion
   }
}