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

		public Rule Configure(Rule rule)
		{
			return InnerConfigure(rule.Message(ErrorMessage));
		}

		protected abstract Rule InnerConfigure(Rule rule);

		#endregion
	}
}
