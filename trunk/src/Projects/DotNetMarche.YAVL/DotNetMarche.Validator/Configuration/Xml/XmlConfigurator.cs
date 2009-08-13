using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DotNetMarche.Validator.Validators;

namespace DotNetMarche.Validator.Configuration.Xml
{
	public class XmlConfigurator
	{
		private XElement Configuration;

		public XmlConfigurator(String configurationFileName)
		{
			Configuration = XElement.Load(configurationFileName);
		}

		/// <summary>
		/// Create the first validator found into configuration file.
		/// </summary>
		/// <returns></returns>
		public Core.Validator CreateValidator()
		{
			Core.Validator validator = new Core.Validator();
			XElement validatorNode = Configuration.Elements("validator").First();
			foreach (XElement typeNode in validatorNode.Elements("type"))
			{
				Type currentType = Type.GetType(typeNode.Attribute("name").Value);
				if (currentType == null)
					throw new ConfigurationException("Configuration node " + typeNode.ToString() + " validate type " +
						typeNode.Attribute("name").Value + " that does not exists");
				//A node can have one or more rules, rules are simply extractor with inner validators
				foreach (XElement ruleNode in typeNode.Elements())
				{
					IExtratorNode node = XmlHelper.GetExtractorNode(ruleNode);
					Rule rule = Rule.For(currentType);
					node.Configure(rule);
					validator.AddRule(rule);
				}
			}
			return validator;
		}
	}
}