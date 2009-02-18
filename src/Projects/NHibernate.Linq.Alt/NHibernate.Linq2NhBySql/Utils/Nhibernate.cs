using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Expressions;

namespace NHibernate.Linq.Utils
{
   static class NHibernate
    {
       internal static void AddProjectionToCriteria(IProjection projection, ICriteria criteria) {
            if (criteria.Projection == null)
                criteria.SetProjection(Projections.ProjectionList());
           ((ProjectionList) criteria.Projection).Add(projection);
       }
    }
}
