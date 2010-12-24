using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DotNetMarche.Validator.Core;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Concrete;
using DotNetMarche.Validator.ValueExtractors;

namespace DotNetMarche.Validator.Validators
{
	public partial class Rule
	{
		#region Inner functions and properties

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

		internal Rule Configure(Core.Validator validator)
		{
			ValidationUnitCollection coll = validator.GetRules(Type);
			coll.Add( ValidationUnit.CreateValidationUnit(ErrorMessage, _CreateValidator(Extractor)));
			return this;
		}

		#endregion

		#region Validation Fluent Start interface

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

		public static Rule ForMember<T>(Expression<Func<T, Object>> propertyExtractor)
		{
			//we can have directly a member expression or a convert expression
			PropertyInfo pinfo;
			MemberExpression mex;
			if (propertyExtractor.Body is UnaryExpression)
			{
				UnaryExpression exp = (UnaryExpression)propertyExtractor.Body;
				if (exp.NodeType != ExpressionType.Convert)
				{
					throw new ArgumentException("The expression is not a property selector", "propertyExtractor");
				}
				mex = exp.Operand as MemberExpression;
			}
			else
			{
				mex = propertyExtractor.Body as MemberExpression;
			}

			if (mex == null) throw new ArgumentException("The expression is not a property selector", "propertyExtractor");
			Rule rule = For(typeof(T));
			pinfo = mex.Member as PropertyInfo;
			if (pinfo == null) throw new ArgumentException("The expression is not a property selector", "propertyExtractor");
			
			rule._Extractor = new PropertyInfoValueExtractor(pinfo);
			return rule;
		}

		public static Rule For(Type type)
		{
			return new Rule() { Type = type };
		}

		#endregion

		#region Message

		public Rule Message(String message)
		{
			ErrorMessage = new ErrorMessage(message);
			return this;
		}

		public Rule Message(String message, Type resourceManagerTypeName)
		{
			ErrorMessage = new ErrorMessage(message, resourceManagerTypeName);
			return this;
		}

		public Rule Message(Expression<Func<String>> messageLambda)
		{
			ErrorMessage = new ErrorMessage(messageLambda);
			return this;
		}

		#endregion

		#region Validation Rule Addition

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

		public Rule IsInRange(Double min, Double max)
		{
			_CreateValidator = e => new RangeValueValidator(e, min, max);
			return this;
		}

		public Rule MaxLength(Int32 maxLength)
		{
			_CreateValidator = e => new RangeLengthValidator(e, maxLength);
			return this;
		}

		public Rule LengthInRange(Int32 minLength, Int32 maxLength)
		{
			_CreateValidator = e => new RangeLengthValidator(e, minLength, maxLength);
			return this;
		}

		public Rule Custom<T>(Func<T, Boolean> validationFunction)
		{
			_CreateValidator = e => new ActionValidation<T>(e, validationFunction);
			return this;
		}

		#endregion
	}
}
