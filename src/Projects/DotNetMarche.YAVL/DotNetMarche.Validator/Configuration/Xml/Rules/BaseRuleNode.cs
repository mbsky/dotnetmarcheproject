using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DotNetMarche.Validator.Validators;

namespace DotNetMarche.Validator.Configuration.Xml.Rules
{
	public abstract class BaseRuleNode : IRuleNode
	{
		[XmlAttribute("errorMessage")]
		public String ErrorMessage { get; set; }

		#region IRuleNode Members

		public Rule Configure(Type messageResourceType, Rule rule)
		{
			if (messageResourceType == null)
			return InnerConfigure(rule.Message(ErrorMessage));
			else

				return InnerConfigure(rule.Message(ErrorMessage, messageResourceType));
			
		}

		protected abstract Rule InnerConfigure(Rule rule);

		#endregion
	}
}
