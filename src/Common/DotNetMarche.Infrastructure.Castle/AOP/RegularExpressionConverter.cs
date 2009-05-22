using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Castle.MicroKernel.SubSystems.Conversion;

namespace DotNetMarche.Infrastructure.Castle.AOP
{
    class RegularExpressionConverter : ITypeConverter {

        #region ITypeConverter Members

        public bool CanHandleType(Type type, global::Castle.Core.Configuration.IConfiguration configuration) {
            if (type == typeof(Regex))
                return true;
            return false;
        }

        public bool CanHandleType(Type type) {
            if (type == typeof(Regex))
                return true;
            return false;
        }

        public ITypeConverterContext Context {
            get {
                return _context;
            }
            set {
                _context = value;
            }
        }
        private ITypeConverterContext _context;

        public object PerformConversion(global::Castle.Core.Configuration.IConfiguration configuration, Type targetType) {
            return new Regex(configuration.Value);
        }

        public object PerformConversion(string value, Type targetType) {
            return new Regex(value);
        }

        #endregion
    }
}