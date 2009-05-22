using System;
using System.Configuration;
using System.Text.RegularExpressions;
using Castle.MicroKernel.SubSystems.Conversion;

namespace DotNetMarche.Infrastructure.Castle
{
    public class RegexConverter : AbstractTypeConverter  
    {
        private static Regex parser = new Regex(@"\{(?<rex>.*)\},{(?<opt>.*)}");

        public override bool CanHandleType(Type type)
        {
            return type == typeof (Regex);
        }

        public override object PerformConversion(global::Castle.Core.Configuration.IConfiguration configuration, Type targetType)
        {
            return PerformConversion(configuration.Value, targetType);
        }

        public override object PerformConversion(string value, Type targetType)
        {
            Match m = parser.Match(value);
            if (!m.Success)
                throw new ArgumentException(
                    String.Format("Cannot convert the string {0} to Regex. String must be in the form {{regex}},{{options}}",value));
            String[] options = m.Groups["opt"].Value.Split(',');
            RegexOptions opt = RegexOptions.None;
            foreach(String option in options) {
                opt |= (RegexOptions) Enum.Parse(typeof(RegexOptions), option);
            }
            return new Regex(m.Groups["rex"].Value, opt);
        }
    }
}