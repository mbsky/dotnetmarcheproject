using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Core;
using Castle.Core.Configuration;
using Castle.Core.Interceptor;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.SubSystems.Conversion;
using DotNetMarche.Infrastructure.Castle.AOP.Helpers;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    public class PolicyFacility : AbstractFacility {

        /// <summary>
        /// Keeps track of all advices
        /// </summary>
        IList<AdviceHandler> advices = new List<AdviceHandler>();

        #region pointcuts handling

        #endregion

        protected override void Init() {
            SetupConfiguration();
            IConversionManager cm = (IConversionManager)Kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
            cm.Add(new RegularExpressionConverter());
            Kernel.AddComponent("policy.intercepter", typeof(AdvisorInterceptor));
            Kernel.ComponentRegistered += new global::Castle.MicroKernel.ComponentDataDelegate(ComponentRegistered);
        }

        private void SetupConfiguration() {
            foreach (IConfiguration child in FacilityConfig.Children) {

                AdviceHandler advice = new AdviceHandler();
                advices.Add(advice);
                foreach (IConfiguration configuration in child.Children) {
                    switch (configuration.Name) {
                        case "pointcut":
                            advice.PointcutComponents.Add(configuration.Attributes["component-id"], false);
                            break;
                        case "aspect":
                            advice.AspectComponents.Add(configuration.Attributes["component-id"], false);
                            break;
                        default:
                            throw new InvalidOperationException("Only <pointcuts> and <aspects> sub elements are allowed");
                    }
                }
            }
        }

        private Boolean AllPointcutAndAspectLoaded = false;
        private List<IHandler> pendingHandlers = new List<IHandler>();

        /// <summary>
        /// For each component register the handler of the component and then check if all
        /// the pointcut and handlers are already scanned
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        void ComponentRegistered(string key, global::Castle.MicroKernel.IHandler handler) {
            if (!AllPointcutAndAspectLoaded) {
                ScanForPointcutsOrAspects(handler);
            } else {
                ScanHandlerForPointcuts(handler);
            }

			
        }

        private void ScanHandlerForPointcuts(IHandler handler) {
            foreach (AdviceHandler advice in advices) {
                if (advice.TypeContainPointcut(handler.ComponentModel.Service, Kernel)) {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AdvisorInterceptor)));
                    handler.ComponentModel.ExtendedProperties["advices"] = advices;
                    handler.ComponentModel.ExtendedProperties["kernel"] = Kernel;
                    break;
                }
            }
        }

        /// <summary>
        /// While we have not loaded all pointcuts and handler there is no way 
        /// to know if a class should be proxyed, so all pending values are stored.
        /// </summary>
        /// <param name="handler"></param>
        private void ScanForPointcutsOrAspects(IHandler handler) {
            //First check if the object is a pointcut or an aspect.
            if (handler.ComponentModel.Service.IsAssignableFrom(typeof(IPointcut))) {
                foreach (AdviceHandler advice in advices) {
                    if (advice.PointcutComponents.ContainsKey(handler.ComponentModel.Name))
                        advice.PointcutComponents[handler.ComponentModel.Name] = true;
                }
            }
            else if (handler.ComponentModel.Service.IsAssignableFrom(typeof(IAspect))) {
                foreach (AdviceHandler advice in advices) {
                    if (advice.AspectComponents.ContainsKey(handler.ComponentModel.Name))
                        advice.AspectComponents[handler.ComponentModel.Name] = true;
                }
            }
            else {
                pendingHandlers.Add(handler);
            }
            //now check if we finished 
            foreach(AdviceHandler advice in advices) 
                if (!advice.ComponentsLoadFinished)
                    return;
            //If we reach here loading is finished.
            foreach (IHandler pendingHandler in pendingHandlers)
                ScanHandlerForPointcuts(pendingHandler);
            AllPointcutAndAspectLoaded = true;
            pendingHandlers.Clear();
        }
    }
}