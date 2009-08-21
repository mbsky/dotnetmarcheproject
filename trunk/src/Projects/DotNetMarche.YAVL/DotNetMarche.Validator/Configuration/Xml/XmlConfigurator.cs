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
			Type resourceType = null;
			XElement resourceTypeNameNode = validatorNode.Elements("resource").FirstOrDefault();
			if (resourceTypeNameNode != null)
			{
				String resourceTypeName = resourceTypeNameNode.Attribute("name").Value;

				resourceType = Type.GetType(resourceTypeName);
				if (resourceType == null)
				{
					throw new ConfigurationErrorsException(
						String.Format("The type {0} specified for resource is not valid", resourceTypeName));
				}
			}
			foreach (XElement typeNode in validatorNode.Elements("type"))
			{
				Type currentType = Type.GetType(typeNode.Attribute("name").Value);
				if (currentType == null)
					throw new ConfigurationErrorsException("Configuration node " + typeNode.ToString() + " validate type " +
						typeNode.Attribute("name").Value + " that does not exists");
				//A node can have one or more rules, rules are simply extractor with inner validators
				foreach (XElement ruleNode in typeNode.Elements())
				{
					IExtratorNode node = XmlHelper.GetExtractorNode(ruleNode);
					node.Configure(resourceType, currentType, validator);
				}
			}
			return validator;
		}
	}
}