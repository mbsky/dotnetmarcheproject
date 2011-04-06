using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DotNetMarche.Validator.Configuration.Xml.Rules
{
	[Serializable, XmlRoot("range", Namespace="")]
	public class RangeRuleNode : BaseRuleNode
	{
		[XmlAttribute("min")]
		public Double Min { get; set; }

		[XmlAttribute("max")]
		public Double Max { get; set; }

		#region RuleNode Members

		protected override Validators.Rule InnerConfigure(Validators.Rule rule)
		{
			return rule.IsInRange(Min, Max);
		}

		#endregion
	}
}
