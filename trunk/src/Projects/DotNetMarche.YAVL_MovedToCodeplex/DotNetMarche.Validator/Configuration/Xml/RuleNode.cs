using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Validators;

namespace DotNetMarche.Validator.Configuration.Xml
{
	public interface IRuleNode
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="messageResourceType">This parameter can be null if no localization is chosen. If present it
		/// represents the localization class to take messages from.</param>
		/// <param name="rule"></param>
		/// <returns></returns>
		Rule Configure(Type messageResourceType, Rule rule);
	}
}
