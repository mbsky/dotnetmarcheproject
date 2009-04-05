using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Helpers;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic.Data;

namespace DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic
{
   public class MasterModel
   {
      private XElement _menuData;

      /// <summary>
      /// Key is text, and value is the url pointing to.
      /// </summary>
      public XElement MenuData
      {
         get { return _menuData; }
      }

      internal MenuItem CreateMenu(String menuFileName)
      {
         MenuItem menu = Cache.Get<MenuItem>("mainmenu");

         if (menu == null)
         {
            XDocument doc = XDocument.Load(menuFileName);
            MenuItem root = CreateMenuItem(doc.Root);
            menu = root;
            Cache.Insert("mainmenu", menu, new TimeSpan(1, 0, 0, 0));
         }
         return menu;
      }

      /// <summary>
      /// Create the menu item for this element, the element 
      /// </summary>
      /// <param name="element"></param>
      public MenuItem CreateMenuItem(XElement element)
      {
         //iterate into all node of the element, and create appropriate node.
         var innerMenu = from XElement node in element.Elements()
                         select CreateMenuItem(node);

         if (element.Name == "menu")
            return new MenuItem("Menu", innerMenu.ToList());
         else if (element.Name == "url")
            return new MenuLink(element.Attribute("text").Value, element.Attribute("url").Value);
         else if (element.Name == "submenu")
            return new MenuItem(element.Attribute("text").Value, innerMenu.ToList());
         else if (element.Name == "action")
            return new MenuLink(
               element.Attribute("text").Value, 
               "/" + element.Attribute("controller").Value 
               + "/" + element.Attribute("action").Value);
         throw new NotSupportedException("The element " + element.Name + " is not supported on the menu.");
         
      }
   }
}