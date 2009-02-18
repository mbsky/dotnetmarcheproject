using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace ExpressionTreeDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Windows.Forms.Application.Run(new Dumper());
            
        }
    }
}
