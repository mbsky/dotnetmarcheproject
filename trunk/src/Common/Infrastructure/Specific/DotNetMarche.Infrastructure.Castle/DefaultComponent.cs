using System;
using System.Collections.Generic;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.SubSystems.Naming;

namespace DotNetMarche.Infrastructure.Castle
{
    public class DefaultComponent : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.ComponentRegistered += OnComponentRegistered;
            Kernel.AddSubSystem(SubSystemConstants.NamingKey, new NamingSubsystemForDefault(defaults));
        }

        private Dictionary<Type, IHandler> defaults = new Dictionary<Type, IHandler>();
        private void OnComponentRegistered(string key, IHandler handler)
        {
            if (handler.ComponentModel.Configuration != null && handler.ComponentModel.Configuration.Attributes["default"] == "true")
            {
                defaults.Add(handler.ComponentModel.Service, handler);
            }
        }
    }

    internal class NamingSubsystemForDefault : DefaultNamingSubSystem
    {
        private Dictionary<Type, IHandler> defaults = new Dictionary<Type, IHandler>();

        public NamingSubsystemForDefault(Dictionary<Type, IHandler> defaults)
        {
            this.defaults = defaults;
        }

        public override IHandler GetHandler(Type service)
        {
            if (defaults.ContainsKey(service))
                return defaults[service];

            return base.GetHandler(service);
        }
    }
}