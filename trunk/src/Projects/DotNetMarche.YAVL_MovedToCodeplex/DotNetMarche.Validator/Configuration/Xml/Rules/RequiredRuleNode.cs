using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DotNetMarche.Validator.Configuration.Xml.Rules
{
	[Serializable, XmlRoot("required", Namespace="")]
	public class RequiredRuleNode : BaseRuleNode
	{
		#region RuleNode Members

		protected override Validators.Rule InnerConfigure(Validators.Rule rule)
		{
			return rule.Required();
		}

		#endregion
	}
}
