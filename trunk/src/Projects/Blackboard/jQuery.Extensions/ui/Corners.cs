using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jQuery.Extensions.ui
{
    // ui.theme.css
    public class Corners
    {
        public enum CornersType
        {
            tl,
            tr,
            bl,
            br,
            top,
            bottom,
            right,
            left,
            all
        }

        public CornersType Name { get; set;}

        public Corners() : this(CornersType.all)
        {
        
        }

        public Corners(CornersType corner)
        {
            Name = corner;
        }

        public string ToCss()
        {
            return string.Format("ui-corner-{0}", Name);
        }
    }
}
