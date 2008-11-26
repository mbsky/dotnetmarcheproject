using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Visitors
{
    abstract class Walker
    {
       protected ICriteria rootCriteria;
     
       protected Walker(ICriteria criteria) {
            rootCriteria = criteria;
       }
       
    }
}
