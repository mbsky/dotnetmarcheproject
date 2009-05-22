//using System;
//using System.Collections.Generic;
//using System.Text;
//using Castle.MicroKernel;
//using Nablasoft.Castle.Windsor.AOP.Tests.TestSuite1.ExtClasses;

//namespace Nablasoft.Castle.Windsor.AOP.Tests.HelperClasses {
//    public class SubDependencyResolver : ISubDependencyResolver  {
//        #region ISubDependencyResolver Members

//        public bool CanResolve(CreationContext context, ISubDependencyResolver parentResolver, global::Castle.Core.ComponentModel model, global::Castle.Core.DependencyModel dependency) {
//            return true;
//        }

//        public object Resolve(CreationContext context, ISubDependencyResolver parentResolver, global::Castle.Core.ComponentModel model, global::Castle.Core.DependencyModel dependency) {
//            return new SomeClass();
//        }

//        #endregion
//    }
//}
