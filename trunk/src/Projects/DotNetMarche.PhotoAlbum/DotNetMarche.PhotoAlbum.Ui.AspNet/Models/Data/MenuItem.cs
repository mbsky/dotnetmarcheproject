using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.Models.Data
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
   }

   public class MenuLink : MenuItem 
   {
      public MenuLink(string text, string url) : base(text)
      {
         Url = url;
      }
      public String Url { get; set; }
   }
}
