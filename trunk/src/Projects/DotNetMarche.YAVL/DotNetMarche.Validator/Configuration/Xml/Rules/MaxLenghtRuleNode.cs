using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DotNetMarche.Validator.Configuration.Xml.Rules
{
	[Serializable, XmlRoot("maxlength", Namespace = "")]
	public class MaxLenghtRuleNode : BaseRuleNode
	{

		[XmlAttribute("maxlength")]
		public Int32 MaxLenght { get; set; }

		#region RuleNode Members

		protected override Validators.Rule InnerConfigure(Validators.Rule rule)
		{
			return rule.MaxLength(MaxLenght);
		}

		#endregion
	}
}
