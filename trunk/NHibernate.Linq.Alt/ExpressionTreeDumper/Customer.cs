using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionTreeDumper
{
    public class Customer
    {
        public String field;
        public String property;

        public String Property {
            get { return property; }
            set { property = value; }
        }
    }
}
