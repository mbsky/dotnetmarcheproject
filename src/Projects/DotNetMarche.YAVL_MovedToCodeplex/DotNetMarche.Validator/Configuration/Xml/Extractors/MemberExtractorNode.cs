using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using DotNetMarche.Validator.Configuration.Xml.Rules;
using DotNetMarche.Validator.Validators;

namespace DotNetMarche.Validator.Configuration.Xml.Extractors
{
	[Serializable, XmlRoot("member")]
	public class MemberExtractorNode : BaseExtractor
	{

		public String Name { get; set; }

		#region ExtratorNode Members

		protected override  Rule InnerConfigure(Rule rule)
		{
			return rule.OnMember(Name);
		}

		#endregion

		protected override void ReadXml(XElement element)
		{
			Name = element.Attribute("name").Value;
		}

		protected override System.Xml.Linq.XElement WriteXml()
		{
			XElement element = new XElement("member");
			element.Add(new XAttribute("name", Name ?? ""));
			return element;
		}
	}
}