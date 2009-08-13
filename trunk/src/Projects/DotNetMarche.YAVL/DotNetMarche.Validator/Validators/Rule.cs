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
				if (_CreateValidator != null)
					Validator = _CreateValidator(value);
			}
		}
		private IValueExtractor _Extractor;

		private IValidator Validator { get; set; }
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
			if (Extractor == null)
				_CreateValidator = e => new RequiredValidator(e);
			else
				Validator = new RequiredValidator(Extractor);
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
			coll.Add(new ValidationUnit(ErrorMessage, Validator));
			return this;
		}

		public Rule IsInRange(Double min, Double max)
		{
			if (Extractor == null)
				_CreateValidator = e => new RangeValueValidator(e, min, max);
			else
				Validator = new RangeValueValidator(Extractor, min, max);
			return this;
		}
	}
}
