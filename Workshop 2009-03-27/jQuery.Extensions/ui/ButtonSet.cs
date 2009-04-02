using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace jQuery.Extensions.ui
{
    public class ButtonSet
    {
        public IList<ButtonBase> Buttons { get; private set; }
        private readonly Toolbar _toolbar;

        public enum ButtonSetType { Normal, Multi, Single }
        public ButtonSetType SetType { get; set; }

        public ButtonSet(Toolbar toolbar)
            : this(toolbar, ButtonSetType.Normal)
        {
        }

        public ButtonSet(Toolbar toolbar, ButtonSetType type)
        {
            this.SetType = type;
            this._toolbar = toolbar;
            this.Buttons = new List<ButtonBase>();
        }

        public Toolbar EndSet()
        {
            return this._toolbar;
        }

        public ButtonSet AddButton(string id, string title)
        {
            return AddButton(id, title, false);
        }

        public ButtonSet AddButton(string id, string title, bool disabled)
        {
            this.Buttons.Add(new Button(id, title, disabled));
            return this;
        }

        public ButtonSet AddLinkButton(string id, string title, bool disabled)
        {
            this.Buttons.Add(new LinkButton(id, title, false));
            return this;
        }

        public ButtonSet AddLinkButton(string id, string title)
        {
            return AddLinkButton(id, title, true);
        }

        public ButtonSet AddIconButton(string id, string title, string icon)
        {
            return AddIconButton(id, title, icon, false);
        }

        public ButtonSet AddIconButton(string id, string title, string icon, bool disabled)
        {
            this.Buttons.Add(new LinkButton(id, title, false) { Icon = icon });
            return this;
        }

        public ButtonSet AddIconSolo(string id, string title, string icon, bool disabled)
        {
            this.Buttons.Add(new LinkButton(id, title, disabled) { Icon = icon, IsIconSolo = true});
            return this;
        }

        public ButtonSet AddIconSolo(string id, string title, string icon)
        {
            return AddIconSolo(id, title, icon, false);
        }
        
        public void Render(HtmlTextWriter writer)
        {
            switch (this.SetType)
            {
                case ButtonSetType.Normal:
                    writer.AddAttribute("class", "fg-buttonset ui-helper-clearfix");
                    break;
                case ButtonSetType.Multi:
                    writer.AddAttribute("class", "fg-buttonset fg-buttonset-multi");
                    break;
                case ButtonSetType.Single:
                    writer.AddAttribute("class", "fg-buttonset ui-helper-clearfix fg-buttonset-single");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            writer.RenderBeginTag("div");

            for (int c = 0; c < Buttons.Count; c++ )
            {
                ButtonBase button = Buttons[c];
                if (SetType != ButtonSetType.Normal)
                {
                    if( Buttons.Count == 1)
                    {
                        button.CornerStyle = ButtonBase.Corner.all;
                    }
                    else
                    {
                        if (c == 0)
                            button.CornerStyle = ButtonBase.Corner.left;
                        else if (c == Buttons.Count - 1)
                            button.CornerStyle = ButtonBase.Corner.right;
                        else
                            button.CornerStyle = ButtonBase.Corner.none;
                    }
                }

                button.Render(writer);
            }

            writer.RenderEndTag();
        }
    }
}
