using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.EnterpriseLibrary
{
   class UnityIoCEngine : IInversionOfControlContainer
   {
      #region IInversionOfControlContainer Members

      public T Resolve<T>()
      {
         throw new NotImplementedException();
      }

      public T Resolve<T>(string objectName)
      {
         throw new NotImplementedException();
      }

      public T Resolve<T>(params object[] values)
      {
         throw new NotImplementedException();
      }

      public T ResolveWithName<T>(string name, params object[] values)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
