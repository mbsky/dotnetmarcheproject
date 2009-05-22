using System;
using Castle.Core;

namespace DotNetMarche.Infrastructure.Castle
{
    /// <summary>
    /// implementa un attributo da inserire sulle interfacce per la registrazione automatica.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=true, AllowMultiple=false)]
    public class AutoscanComponentAttribute : Attribute {

        /// <summary>
        /// L'id di registrazione.
        /// </summary>
        public String Id
        {
            get { return id; }
            set { id = value; }
        }
        private String id;

        /// <summary>
        /// Il tipo di servizio, se nullo significa che l'oggetto implementa una sola
        /// interfaccia ed allora non ha bisogno di specificare nulla
        /// </summary>
        public Type ServiceType
        {
            get { return serviceType; }
            set { serviceType = value; }
        }
        private Type serviceType;   
          

        public LifestyleType LifestyleType {
            get { return lifestyleType; }
            set { lifestyleType = value; }
        }
        private LifestyleType lifestyleType = LifestyleType.Singleton;

        /// <summary>
        /// Indica se l'istanza è di default
        /// </summary>
        public Boolean IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
        private Boolean isDefault;


        public AutoscanComponentAttribute(LifestyleType lifestyleType) : this(lifestyleType, false) {

        }

        public AutoscanComponentAttribute(LifestyleType lifestyleType, bool isDefault) {
            this.lifestyleType = lifestyleType;
            this.isDefault = isDefault;
        }
    }
}