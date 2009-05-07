using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcHelper;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic;
using DotNetMarche.PhotoAlbum.Ui.AspNet.MvcLogic.Data;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;


namespace DotNetMarche.PhotoAlbum.Ui.Test.Mvc.Models
{
   [TestFixture]
   public class MasterModelModelTest
   {
private class MyTestUrlHelper : IUrlHelper
{

   #region IUrlHelper Members

   public string RouteUrl(RouteValueDictionary values)
   {
      return RouteUrl((IDictionary<String, Object>)values);
   }

   public string RouteUrl(object values)
   {
      IDictionary<String, Object> dic = (IDictionary<String, Object>) values;
      return "/" + dic["controller"] + "/" + dic["action"];
   }

   public string RouteUrl(string routeName, object values)
   {
      return RouteUrl(values);
   }

   #endregion
}


[Test]
public void GrabBasicMenu()
{
   MasterLogic sut = new MasterLogic(new MyTestUrlHelper());
   List<MenuItem> menu = sut.CreateMenu("SampleFiles\\BaseMenu1.Xml").MenuItems;
   Assert.That(menu, Has.Count(2));
   Assert.That(menu[0], Has.Property("Url", "/Login.aspx"));
   Assert.That(menu[0], Has.Property("Text", "Login Page"));
}

      [Test]
      public void GrabMenuWithSubMenu()
      {
         MasterLogic sut = new MasterLogic(new MyTestUrlHelper());
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuH1.Xml").MenuItems;
         Assert.That(menu, Has.Count(1));
         Assert.That(menu[0], Has.Property("Text", "administration"));
         Assert.That(menu[0].MenuItems, Has.Count(2));
      }

      [Test]
      public void GrabMenuWithSubMenuTypes()
      {
         MasterLogic sut = new MasterLogic(new MyTestUrlHelper());
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuH1.Xml").MenuItems;
         Assert.That(menu, Has.Count(1));
         Assert.That(menu[0], Is.TypeOf(typeof(MenuItem)));
         Assert.That(menu[0].MenuItems[0], Is.TypeOf(typeof(MenuLink)));
         Assert.That(menu[0].MenuItems[1], Is.TypeOf(typeof(MenuLink)));
      }

      [Test]
      public void GrabMenuWithSubMenuUrlAndText()
      {
         MasterLogic sut = new MasterLogic(new MyTestUrlHelper());
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuH1.Xml").MenuItems;
         Assert.That(menu[0].MenuItems[0].Text, Is.EqualTo("Login Page"));
         Assert.That(menu[0].MenuItems[1].Text, Is.EqualTo("Registration Page"));
         Assert.That(menu[0].MenuItems[0], Has.Property("Url", "/Login.aspx"));
         Assert.That(menu[0].MenuItems[1], Has.Property("Url", "/CreateUser.aspx"));
      }

      [Test]
      public void GrabMenuWithAction()
      {
         MasterLogic sut = new MasterLogic(new MyTestUrlHelper());
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuType1.Xml").MenuItems;
         Assert.That(menu, Has.Count(2));
         Assert.That(menu[1].MenuItems, Has.Count(1));
      }

[Test]
public void GrabMenuWithActionUrl()
{
   MasterLogic sut = new MasterLogic(new MyTestUrlHelper());
   List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuType1.Xml").MenuItems;
   Assert.That(menu, Has.Count(2));
   Assert.That(menu[1].MenuItems[0], Has.Property("Url", "/Photo/ManageAlbum"));
}
   }
}
