using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using jQuery.Extensions.ui;

namespace jQuery.Extensions.Helpers
{
    public static class jQueryUIExtender
    {
        private static readonly jQueryUI _ui;

        static jQueryUIExtender()
        {
            _ui = new jQueryUI();
        }


        public static jQueryUI jQueryUI(this HtmlHelper Html)
        {
            return _ui;
        }
    }

    public class jQueryUI
    {
        public string LinkButton(string id, string icon)
        {
            return LinkButton(id, null, icon, false, true);
        }

        public string LinkButton(string id, string text, string icon)
        {
            return LinkButton(id, text, icon, false, false);
        }

        public string LinkButton(string id, string text, string icon, bool disabled, bool iconSolo)
        {
            var btn = new LinkButton(id, text, disabled) { Icon = icon, IsIconSolo = iconSolo };
            return Render(btn);
        }

        private static string Render(IHtmlComponent component)
        {
            var textWriter = new StringWriter();
            var  writer = new HtmlTextWriter(textWriter);
            component.Render(writer);
            return textWriter.ToString();
        }
    }
}
