using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace jQuery.Extensions.ui
{
    public class Toolbar
    {
        public IList<ButtonSet> ButtonSets { get; private set; }
        public Corners Corners { get; set; }
        private readonly bool IsHollow;
        
        public Toolbar(bool hollow)
        {
            this.Corners = new Corners();
            this.IsHollow = hollow;
            this.ButtonSets = new List<ButtonSet>();
        }

        public static Toolbar Create()
        {
            return new Toolbar(false);
        }

        public Toolbar SetCorners(Corners.CornersType corner)
        {
            this.Corners.Name = corner;
            return this;
        }

        public static Toolbar CreateHollow()
        {
            return new Toolbar(true);
        }

        public void Render(HtmlTextWriter writer)
        {
            if (!IsHollow)
            {
                writer.AddAttribute("class", string.Format("fg-toolbar ui-widget-header {0} ui-helper-clearfix", Corners.ToCss()));
                writer.RenderBeginTag("div");
            }

            foreach (var set in ButtonSets)
            {
                set.Render(writer);
            }

            if (!IsHollow)
                writer.RenderEndTag();
        }

        public ButtonSet AddSet()
        {
            ButtonSet set = new ButtonSet(this);
            this.ButtonSets.Add(set);
            return set;
        }

        public ButtonSet AddToggle()
        {
            ButtonSet set = new ButtonSet(this, ButtonSet.ButtonSetType.Multi);
            this.ButtonSets.Add(set);
            return set;
        }

        public ButtonSet AddSingle()
        {
            ButtonSet set = new ButtonSet(this, ButtonSet.ButtonSetType.Single);
            this.ButtonSets.Add(set);
            return set;
        }

        
        public string Render()
        {
            StringWriter stringWriter = new StringWriter();

            HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

            Render(writer);

            return stringWriter.ToString();
        }
    }
}
