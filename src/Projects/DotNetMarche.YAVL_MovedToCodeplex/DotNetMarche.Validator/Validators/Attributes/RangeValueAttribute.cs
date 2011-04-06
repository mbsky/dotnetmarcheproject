using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator.BaseClasses;
using DotNetMarche.Validator.Interfaces;
using DotNetMarche.Validator.Validators.Concrete;

namespace DotNetMarche.Validator.Validators.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
	public class RangeValueAttribute : BaseValidationAttribute {

		#region Constructors

		Double mMinValue;
		Double mMaxValue;

		private void Init(
			Double minValue,
			Double maxValue) {
			mMinValue = minValue;
			mMaxValue = maxValue;
			}

		public RangeValueAttribute(
			String errorMessage, 
			Double minValue,
			Double maxValue) : this(errorMessage, null, minValue, maxValue) {} 

		public RangeValueAttribute(
			String errorMessage, 
			String resourceTypeName, 
			Double minValue,
			Double maxValue) : base(errorMessage, resourceTypeName) {

			Init(minValue, maxValue);
			} 


		#endregion

		public override IValidator CreateValidator(IValueExtractor valueExtractor) {
			return new RangeValueValidator(valueExtractor, mMinValue, mMaxValue) ; 
		}
	}
}