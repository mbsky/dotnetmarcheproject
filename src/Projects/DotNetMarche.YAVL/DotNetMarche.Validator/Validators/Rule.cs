using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Concrete;
using DotNetMarche.Validator.ValueExtractors;

namespace DotNetMarche.Validator.Validators
{
	public partial class Rule
	{
		private Type Type { get; set; }
		private IValueExtractor Extractor
		{
			get
			{
				return _Extractor;
			}
			set
			{
				_Extractor = value;
			}
		}
		private IValueExtractor _Extractor;
		private ErrorMessage ErrorMessage { get; set; }
		private Func<IValueExtractor, IValidator> _CreateValidator;

		public static Rule For<T>()
		{
			return For(typeof(T));
		}		
		
		public static Rule For<T>(Func<T, Object> extractor) 
		{
			Rule rule = For(typeof(T));
			rule._Extractor = new LambdaExtractor<T>(extractor);
			return rule;
		}

		public static Rule For(Type type)
		{
			return new Rule() { Type = type };
		}



		public Rule OnMember(String propertyName)
		{
			Extractor = new NamedValueExtractor(propertyName);
			return this;
		}

		/// <summary>
		/// Calling this property insert a required validator
		/// </summary>
		public Rule Required
		{
			get
			{
				return SetRequired();
			}
		}

		public Rule SetRequired()
		{
				_CreateValidator = e => new RequiredValidator(e);
			return this;
		}

		public Rule Message(String message)
		{
			ErrorMessage = new ErrorMessage(message); 
			return this;
		}

		public Rule Message(Expression<Func<String>> messageLambda)
		{
			ErrorMessage = new ErrorMessage(messageLambda);
			return this;
		}

		internal Rule Configure(Core.Validator validator)
		{
			ValidationUnitCollection coll = validator.GetRules(Type);
			coll.Add(new ValidationUnit(ErrorMessage, _CreateValidator(Extractor)));
			return this;
		}

		public Rule IsInRange(Double min, Double max)
		{
			_CreateValidator = e => new RangeValueValidator(e, min, max);
			return this;
		}

	}
}
