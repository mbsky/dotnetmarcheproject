using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using DotNetMarche.Validator.Validators;

namespace DotNetMarche.Validator.Configuration.Xml.Extractors
{
	public abstract class BaseExtractor : IXmlSerializable, IExtratorNode
	{
		protected BaseExtractor()
		{
			RuleNodes = new List<IRuleNode>();
		}

		[XmlElement("rules")]
		public List<IRuleNode> RuleNodes { get; set; }
		
		#region IXmlSerializable Members

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			throw new NotImplementedException();
		}

		public void ReadXml(System.Xml.XmlReader reader)
		{
			XElement element = XElement.Load(reader);
			ReadXml(element);
			//now take care of elements
			foreach (XElement ruleElement in element.Elements())
			{
				RuleNodes.Add(XmlHelper.GetRuleNode(ruleElement));
			}
		}

		public  void WriteXml(System.Xml.XmlWriter writer)
		{
			XElement element = WriteXml();
			foreach (IRuleNode list in RuleNodes)
			{
				element.Add(XElement.Parse(XmlHelper.ToXml(list)));
			}
			foreach (XAttribute attribute in element.Attributes())
			{
				writer.WriteAttributeString(attribute.Name.LocalName, attribute.Value);
			}
			writer.WriteRaw((String) element);
		}

		/// <summary>
		/// Function that must be overridden to configure the element.
		/// </summary>
		/// <param name="element"></param>
		protected abstract void ReadXml(XElement element);

		/// <summary>
		/// Function that must be overridden to configure the element.
		/// </summary>
		/// <param name="element"></param>
		protected abstract XElement WriteXml();

		#endregion


		#region IExtratorNode Members

		public Rule Configure(Rule rule)
		{
			foreach (IRuleNode node in RuleNodes)
			{
				node.Configure(rule);
			}
			return rule;
		}

		protected abstract Rule InnerConfigure(Rule rule );
		#endregion
	}
}
