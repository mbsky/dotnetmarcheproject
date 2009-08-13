using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DotNetMarche.Validator.Configuration.Xml.Rules
{
	[Serializable, XmlRoot("required", Namespace="")]
	public class RequiredRuleNode : IRuleNode
	{
		#region RuleNode Members

		public  Validators.Rule Configure(Validators.Rule rule)
		{
			return rule.SetRequired();
		}

		#endregion
	}
}
