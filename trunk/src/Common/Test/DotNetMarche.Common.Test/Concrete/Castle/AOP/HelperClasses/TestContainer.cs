using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Resource;
using Castle.MicroKernel;
using Castle.Windsor.Configuration.Interpreters;

namespace Nablasoft.Castle.Windsor.AOP.Tests.HelperClasses {
	
	/// <summary>
	/// This is a windsor container fakie because we can override how the object gets resolved.
	/// </summary>
	public class TestContainer : global::Castle.Windsor.WindsorContainer {

		private readonly Dictionary<Type, object> mOverrideServices = new Dictionary<Type, object>();
		private readonly Dictionary<String, object> mOverrideServicesNamed = new Dictionary<String, object>();

		public TestContainer(String configFileName)
			: base(configFileName) {
		}

		public void AddServiceOverride(Type type, object obj) {
			mOverrideServices.Add(type, obj);
		}

		public void RemoveServiceOverride(Type type) {
			mOverrideServices.Remove(type);
		}

		public void AddNamedServiceOverride(String serviceName, object obj) {
			mOverrideServicesNamed.Add(serviceName, obj);
		}

		public override object Resolve(Type service) {
			if (mOverrideServices.ContainsKey(service))
				return mOverrideServices[service];
			return base.Resolve(service);
		}
		public override object Resolve(string key) {
			if (mOverrideServicesNamed.ContainsKey(key))
				return mOverrideServicesNamed[key];
			return base.Resolve(key);
		}

		//public DisposableAction OverrideConfiguration(params object[] newconf) {
		//   IKernel k = new global::Castle.MicroKernel.DefaultKernel(
		//      Kernel.Resolver, Kernel.ProxyFactory);
		//   for (Int32 index = 0; index < newconf.Length; index += 2) {
		//      k.AddComponentInstance(newconf[index].ToString(), newconf[index + 1]);
		//   }
			
		//   Kernel.AddChildKernel(k);
		//   return new DisposableAction(delegate() { Kernel.RemoveChildKernel(k);});
		//}
	}
}
