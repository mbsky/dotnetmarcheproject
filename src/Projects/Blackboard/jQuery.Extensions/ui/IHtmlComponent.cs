using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace jQuery.Extensions.ui
{
    public interface IHtmlComponent
    {
        void Render(HtmlTextWriter writer);
    }
}
