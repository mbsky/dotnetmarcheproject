using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
   static class Utils
    {

       static void AddAlias(String path, ICriteria criteria)
       {
           String[] names = path.Split('.');
           for (int i = 0; i < names.Length - 1; i++)
           {
               if (criteria.GetCriteriaByAlias(names[i]) == null)
               {
                   criteria.CreateAlias(names[i], names[i]);
               }
           }
       }
    }
}
