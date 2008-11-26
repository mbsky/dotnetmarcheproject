using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using System.Reflection;
using MbUnit.Core.Graph;

namespace LinqForNHibernate
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MbUnit.Core.AutoRunner auto = new MbUnit.Core.AutoRunner())
            {
                auto.Run();
                auto.ReportToHtml();
            }
        }
    }
}
