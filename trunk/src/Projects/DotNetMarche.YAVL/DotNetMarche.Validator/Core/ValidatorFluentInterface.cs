using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators;

namespace DotNetMarche.Validator.Core
{
	public partial class Validator
	{
		#region Fluent Interface

		public Validator AddRule<T>(IValidator validator, ErrorMessage msg)
		{
			ValidationUnitCollection coll = GetRules<T>();
			coll.Add(new ValidationUnit(msg, validator));
			return this;
		}

		public Validator AddRule(Rule rule)
		{
			rule.Configure(this);
			return this;
		}


		#endregion
	}
}
