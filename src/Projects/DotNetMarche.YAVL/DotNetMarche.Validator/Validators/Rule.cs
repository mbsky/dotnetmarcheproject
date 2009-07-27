using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Concrete;
using DotNetMarche.Validator.ValueExtractors;

namespace DotNetMarche.Validator.Validators
{
	public partial class Rule
	{
		private Type _Type { get; set; }
		private IValueExtractor _Extractor { get; set; }
		private IValidator _Validator { get; set; }
		private ErrorMessage _Message { get; set; }

		public static Rule For<T>()
		{
			return For(typeof (T));
		}

		public static Rule For(Type type)
		{
			return new Rule() {_Type = type};
		}

		public Rule OnMember(String propertyName)
		{
			_Extractor = new NamedValueExtractor(propertyName);
			return this;
		}

		public Rule Required
		{
			get
			{
				_Validator = new RequiredValidator(_Extractor);
				return this;
			}
		}

		public Rule Message(String message)
		{
			_Message = message;
			return this;
		}

		internal Rule Configure(Core.Validator validator)
		{
			ValidationUnitCollection coll = validator.GetRules(_Type);
			coll.Add(new ValidationUnit(_Message, _Validator));
			return this;
		}
	}
}
