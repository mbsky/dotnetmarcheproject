using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace jQuery.Extensions.ui
{
    public abstract class ButtonBase : IHtmlComponent
    {
        public string Text { get; private set; }
        public string ID { get; private set; }
        public bool Disabled { get; set; }
        public Corner CornerStyle { get; set; }
        public bool IsActive { get; set; }
        
        public enum Corner
        {
            none,
            all,
            left,
            right
        }

        protected ButtonBase(string id, string text, bool disabled)
        {
            this.ID = id;
            this.Text = text;
            this.Disabled = disabled;
            this.CornerStyle = Corner.all;
        }

        public abstract void Render(HtmlTextWriter writer);

        public string GetClasses()
        {
            IList<string> classes = new List<string>();
            classes.Add("fg-button");
            classes.Add("ui-state-default");

            if (Disabled)
                classes.Add("ui-state-disabled");

            AddCustomClasses(classes);

            StringBuilder sb = new StringBuilder();
            bool bfirst = true;
            foreach (var s in classes)
            {
                if (bfirst)
                {
                    bfirst = false;
                }
                else
                {
                    sb.Append(" ");
                }

                sb.Append(s);
            }

            return sb.ToString();
        }

        protected virtual void AddCustomClasses(IList<string> classes)
        {
            if(this.CornerStyle != Corner.none)
                classes.Add("ui-corner-" + this.CornerStyle.ToString());

            if(IsActive)
                classes.Add("ui-state-active");
        }
    }
}
