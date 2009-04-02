using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace jQuery.Extensions.ui
{
    public class LinkButton : ButtonBase
    {
        public string Icon { get; set; }
        public bool IsIconSolo { get; set; }
        public LinkButton(string id, string text, bool disabled)
            : base(id, text, disabled)
        {
        }

        public override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", this.ID);
            writer.AddAttribute("href", "#");
            writer.AddAttribute("class", GetClasses());
            writer.RenderBeginTag("a");
            
            if (!string.IsNullOrEmpty(Icon))
            {
                writer.AddAttribute("class", string.Format("ui-icon ui-icon-{0}", Icon));
                writer.RenderBeginTag("span");
                writer.RenderEndTag();
            }
            
            if(this.Text != null)
                writer.Write(this.Text);
            
            writer.RenderEndTag();
        }

        protected override void AddCustomClasses(IList<string> classes)
        {
            base.AddCustomClasses(classes);

            if (!string.IsNullOrEmpty(Icon) && !IsIconSolo)
                classes.Add("fg-button-icon-left");

            if(IsIconSolo)
                classes.Add("fg-button-icon-solo");
        }
    }
}
