using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace DotNetMarche.PhotoAlbum.Ui.Test.Watin
{
    public class WatinBase
    {

        protected void Login(IE ie)
        {
            TextField tf = ie.TextField(Find.ByName(t => t.EndsWith("$UserName")));
            if (!tf.Exists) return; //already logged
            tf.TypeText("Alkampfer");
            ie.TextField(Find.ByName(t => t.EndsWith("$Password"))).TypeText("12345");
            ie.Button(Find.ByName(b => b.EndsWith("$LoginButton"))).Click();
        }
    }
}
