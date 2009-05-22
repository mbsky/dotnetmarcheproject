using System;
using System.Collections.Generic;
using System.Text;
using Castle.MicroKernel;

namespace DotNetMarche.Infrastructure.Castle.AOP.Helpers
{
    /// <summary>
    /// This class keeps track of all declared pointcut and aspectcomponents
    /// defined in an advice
    /// </summary>
    public class AdviceHandler {

        internal Dictionary<String, Boolean> PointcutComponents = new Dictionary<String, Boolean>();
        internal Dictionary<String, Boolean> AspectComponents = new Dictionary<String, Boolean>();

        public Boolean ComponentsLoadFinished {
            get {
                foreach (Boolean pdef in PointcutComponents.Values)
                    if (pdef == false)
                        return false;
                foreach (Boolean aspect in AspectComponents.Values)
                    if (aspect == false)
                        return false;
                return true;
            }
        }

        public bool TypeContainPointcut(Type type, IKernel kernel) {
            foreach (String id in PointcutComponents.Keys) {
                IPointcut pcuts = (IPointcut)kernel.Resolve(
                                                 id, typeof(IPointcut));
                if (pcuts.TypeContainPointcut(type))
                    return true;
            }
            return false;
        }

        public bool IsPointcutMethod(System.Reflection.MethodInfo info, IKernel kernel) {
            foreach (String id in PointcutComponents.Keys) {
                IPointcut pcuts = (IPointcut) kernel.Resolve(
                                                  id, typeof(IPointcut));
                if (pcuts.IsPointcutMethod(info))
                    return true;
            }
            return false;
        }

        public IList<IAspect> CreateAspects(IKernel kernel) {
            List<IAspect> result = new List<IAspect>();
            foreach(String id in AspectComponents.Keys) {
                result.Add((IAspect) kernel.Resolve(id, typeof(IAspect )));
            }
            return result;
        }
    }
}