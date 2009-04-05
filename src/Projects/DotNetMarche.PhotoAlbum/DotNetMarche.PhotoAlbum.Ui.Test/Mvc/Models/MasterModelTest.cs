using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models;
using DotNetMarche.PhotoAlbum.Ui.AspNet.Models.Data;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.PhotoAlbum.Ui.Test.Mvc.Models
{
   [TestFixture]
   public class MasterModelTest
   {

      [Test]
      public void GrabBasicMenu()
      {
         MasterModel sut = new MasterModel();
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\BaseMenu1.Xml").MenuItems;
         Assert.That(menu, Has.Count(2));
         Assert.That(menu[0], Has.Property("Url", "/Login.aspx"));
         Assert.That(menu[0], Has.Property("Text", "Login Page"));
      }

      [Test]
      public void GrabMenuWithSubMenu()
      {
         MasterModel sut = new MasterModel();
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuH1.Xml").MenuItems;
         Assert.That(menu, Has.Count(1));
         Assert.That(menu[0], Has.Property("Text", "administration"));
         Assert.That(menu[0].MenuItems, Has.Count(2));
      }

      [Test]
      public void GrabMenuWithSubMenuTypes()
      {
         MasterModel sut = new MasterModel();
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuH1.Xml").MenuItems;
         Assert.That(menu, Has.Count(1));
         Assert.That(menu[0], Is.TypeOf(typeof(MenuItem)));
         Assert.That(menu[0].MenuItems[0], Is.TypeOf(typeof(MenuLink)));
         Assert.That(menu[0].MenuItems[1], Is.TypeOf(typeof(MenuLink)));
      }

      [Test]
      public void GrabMenuWithSubMenuUrlAndText()
      {
         MasterModel sut = new MasterModel();
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuH1.Xml").MenuItems;
         Assert.That(menu[0].MenuItems[0].Text, Is.EqualTo("Login Page"));
         Assert.That(menu[0].MenuItems[1].Text, Is.EqualTo("Registration Page"));
         Assert.That(menu[0].MenuItems[0], Has.Property("Url", "/Login.aspx"));
         Assert.That(menu[0].MenuItems[1], Has.Property("Url", "/CreateUser.aspx"));
      }

      [Test]
      public void GrabMenuWithAction()
      {
         MasterModel sut = new MasterModel();
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuType1.Xml").MenuItems;
         Assert.That(menu, Has.Count(2));
         Assert.That(menu[1].MenuItems, Has.Count(1));
      }

      [Test]
      public void GrabMenuWithActionUrl()
      {
         MasterModel sut = new MasterModel();
         List<MenuItem> menu = sut.CreateMenu("SampleFiles\\MenuType1.Xml").MenuItems;
         Assert.That(menu, Has.Count(2));
         Assert.That(menu[1].MenuItems[0], Has.Property("Url", "/Photo/ManageAlbum"));
      }
   }
}
