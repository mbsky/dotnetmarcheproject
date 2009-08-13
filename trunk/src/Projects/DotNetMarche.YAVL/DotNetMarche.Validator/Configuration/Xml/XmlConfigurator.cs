using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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
				//A node can have one or more rules.
				foreach (XElement ruleNode in typeNode.Elements())
				{
					//now it is time to check type of node to configure the validator.

				}
			}
			return null;
		}
	}
}