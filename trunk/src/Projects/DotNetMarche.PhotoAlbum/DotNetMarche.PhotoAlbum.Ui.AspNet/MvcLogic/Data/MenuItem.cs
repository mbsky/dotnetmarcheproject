using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic.Data
{
   /// <summary>
   /// A menu is represented by a text and a possibile collection of child menuelement.
   /// </summary>
   public class MenuItem 
   {
      public String Text { get; set; }
      public List<MenuItem> MenuItems { get; set; }

      public MenuItem(string text)
      {
         Text = text;
      }

      public MenuItem(string text, List<MenuItem> menuItems)
      {
         Text = text;
         MenuItems = menuItems;
      }

      public virtual String Render()
      {
         return Text;
      }

   }

   public class MenuLink : MenuItem 
   {
      public MenuLink(string text, string url) : base(text)
      {
         Url = url;
      }
      public String Url { get; set; }

      public override String Render()
      {
         return String.Format(@"<a href=""{0}"">{1}</a>", Url, Text);
      }
   }
}