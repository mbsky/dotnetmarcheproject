using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WatiN.Core;

namespace DotNetMarche.PhotoAlbum.Ui.Test.Watin
{
    [TestFixture]
    public class PhotoAlbumManager : WatinBase
    {
        private IE ie;

        [SetUp]
        public void SetUp()
        {
            ie = new IE();
        }

        [TearDown]
        public void TearDown()
        {
            ie.Dispose();
        }

        /// <summary>
        /// VErify that software ask login for photoalbummangaer page.
        /// </summary>
        [Test]
        public void TestSmokePageAskForLogin()
        {
            ie.GoTo("http://localhost:13164/Photo/PhotoAlbumManager.aspx");
            ie.ClearCookies();
            TextField tf = ie.TextField(Find.ByName(t => t.EndsWith("$UserName")));
            Assert.That(tf.Exists);
            
        }

        [Test]
        public void TestCanLogin()
        {

            ie.GoTo("http://localhost:13164/Login.aspx");
            TextField tf = ie.TextField(Find.ByName(t => t.EndsWith("$UserName")));
            tf.TypeText("Alkampfer");
            ie.TextField(Find.ByName(t => t.EndsWith("$Password"))).TypeText("12345");
            ie.Button(Find.ByName(b => b.EndsWith("$LoginButton"))).Click();
            //Navigate to the other page
            Assert.That(ie.Div("thecontent").Exists);

        }

        [Test]
        public void TestCanSelectAlbum()
        {
            ie.GoTo("http://localhost:13164/Photo/PhotoAlbumManager.aspx");
            base.Login(ie);
            TableCell tc = ie.TableCell(Find.ByText("Sgrinfio Sonnolento"));
            Assert.That(tc.Exists);
            Button b = tc.ContainingTableRow.Button(Find.ByValue("Select"));
            Assert.That(b.Exists);
            b.Click();
            var thumbnails = ie.Divs.Where(d => d.ClassName == "thumbnail");

            Assert.That(thumbnails.Count(), Is.EqualTo(6));
        }

        /// <summary>
        /// Test registered with watin test recorder.
        /// </summary>
        [Test, Explicit]
        public void TestAlterDescription()
        {
            WatiN.Core.IE window = ie;
            ie.GoTo("http://localhost:13164/Photo/PhotoAlbumManager.aspx");
            base.Login(ie);
            Button btn_ctl00ContentPlac = window.Button(Find.ByName("ctl00$ContentPlaceHolder1$PhotoAlbumManager1$grdPhotoAlbum$ctl04$Button1"));
            Assert.That(btn_ctl00ContentPlac.Exists);
            Span spn_ctl00_ContentPla = window.Span(Find.ById("ctl00_ContentPlaceHolder1_PhotoAlbumManager1_frmEdit_rptPhoto_ctl00_SinglePhotoThumbnail1_lblDescription"));
            Assert.That(spn_ctl00_ContentPla.Exists);
            //Added after the registration
            TextField txt_editCurrent = window.TextField(Find.ById("editCurrent"));
            Assert.That(txt_editCurrent.Exists);
            txt_editCurrent.WaitUntilExists(3000);

            window.GoTo("http://localhost:13164/Photo/PhotoAlbumManager.aspx");

            btn_ctl00ContentPlac.Click();

            spn_ctl00_ContentPla.Click();

            txt_editCurrent.Click();

            
            txt_editCurrent.AppendText("!!");
            txt_editCurrent.KeyDown('\n');

        }
    }
}
