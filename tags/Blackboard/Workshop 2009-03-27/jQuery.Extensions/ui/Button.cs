using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace jQuery.Extensions.ui
{
    public class Button : ButtonBase
    {
        public Button(string id, string text, bool disabled)
            : base(id, text, disabled)
        {
        }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", this.ID);
            writer.AddAttribute("type", "submit");
            writer.AddAttribute("class", GetClasses());
            writer.RenderBeginTag("button");
            writer.Write(Text);
            writer.RenderEndTag();
        }
    }
}
