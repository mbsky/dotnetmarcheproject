using System;
using System.Configuration;
using System.Reflection;
using Castle.Core.Configuration;
using Castle.MicroKernel.Facilities;

namespace DotNetMarche.Infrastructure.Castle
{
    /// <summary>
    /// Add a component declared at compiletime
    /// </summary>
    public class AutoscanFacility : AbstractFacility {

        protected override void Init() {
            Configure();
        }

        /// <summary>
        /// Check if the configuration is all ok, if not throw exception
        /// </summary>
        private void Configure() {
            foreach (IConfiguration child in FacilityConfig.Children) {
                if (child.Name == "assembly") {
                    ScanAssembly(Assembly.Load(child.Attributes["name"]));
                }
                else
                    throw new ArgumentException("Unknown element " + child.Name + " in AutoscanFacility configuration");
            }
        }

		

        private void ScanAssembly(Assembly asmtoscan) {
            foreach (Type t in asmtoscan.GetTypes()) {
                Attribute attribute = Attribute.GetCustomAttribute(t, typeof (AutoscanComponentAttribute));
                if (attribute != null)
                {
                    AutoscanComponentAttribute autoscanattribute = attribute as AutoscanComponentAttribute;
                    Type serviceType = autoscanattribute.ServiceType;
                    if (serviceType == null)
                    {
                        serviceType = t.GetInterfaces()[0];
                    }
                    Kernel.AddComponent(
                        autoscanattribute.Id,
                        serviceType,
                        t,
                        autoscanattribute.LifestyleType);
                }
			   
            }
        }
    }
}