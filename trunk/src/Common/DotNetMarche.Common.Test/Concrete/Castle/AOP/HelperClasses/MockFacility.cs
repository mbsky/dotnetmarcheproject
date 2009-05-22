//using System;
//using System.Collections.Generic;
//using System.Text;
//using Castle.Core;
//using Castle.MicroKernel;
//using Castle.MicroKernel.Facilities;

//namespace Nablasoft.Castle.Windsor.AOP.Tests.HelperClasses {

//    /// <summary>
//    /// This facility has only one puropose, return a preconfigured mock instead
//    /// of the standard interceptor.
//    /// </summary>
//    class MockFacility : AbstractFacility {

//        protected override void Init() {
//            //Kernel.Resolver.AddSubResolver(new SubDependencyResolver());
//            Kernel.ComponentCreated += new ComponentInstanceDelegate(ComponentInstanceDelegate);
//            Kernel.ComponentRegistered += new ComponentDataDelegate(ComponentRegistered);
//        }

//        public void ComponentInstanceDelegate(ComponentModel model, Object instance) {
			
//        }

//        private Boolean guard = false;
//        public static Dictionary<String, IAspect> OverrideInterceptors = new Dictionary<string, IAspect>();


//        public void ComponentRegistered(String key, global::Castle.MicroKernel.IHandler handler) {
//            if (OverrideInterceptors.ContainsKey(key) && guard == false) {
//                guard = true;
//                Kernel.RemoveComponent(key);
//                Kernel.AddComponentInstance(key, OverrideInterceptors[key]);
//                guard = false;
//            }
//        }
//    }
//}
