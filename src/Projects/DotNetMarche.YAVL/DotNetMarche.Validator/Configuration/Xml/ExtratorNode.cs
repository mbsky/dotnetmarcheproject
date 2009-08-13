using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Validators;

namespace DotNetMarche.Validator.Configuration.Xml
{
	public interface IExtratorNode
	{
		Rule Configure(Rule rule);
	
	}
}
