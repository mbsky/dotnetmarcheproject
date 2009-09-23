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
			return AddRule(typeof(T), validator, msg);
		}


		public Validator AddRule(Type type, IValidator validator, ErrorMessage msg)
		{
			ValidationUnitCollection coll = GetRules(type);
			coll.Add(new ValidationUnit(msg, validator));
			return this;
		}

		public Validator AddRule(params Rule[] rules)
		{
			foreach (Rule rule in rules)
			{
				rule.Configure(this);
			}
			return this;
		}

		#endregion
	}
}
