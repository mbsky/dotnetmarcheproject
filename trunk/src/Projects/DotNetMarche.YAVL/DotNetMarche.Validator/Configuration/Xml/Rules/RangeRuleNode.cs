using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DotNetMarche.Validator.Configuration.Xml.Rules
{
	[Serializable, XmlRoot("range", Namespace="")]
	public class RangeRuleNode : IRuleNode
	{
		[XmlAttribute("min")]
		public Double Min { get; set; }

		[XmlAttribute("max")]
		public Double Max { get; set; }

		#region RuleNode Members

		public  Validators.Rule Configure(Validators.Rule rule)
		{
			return rule.SetRequired();
		}

		#endregion
	}
}
