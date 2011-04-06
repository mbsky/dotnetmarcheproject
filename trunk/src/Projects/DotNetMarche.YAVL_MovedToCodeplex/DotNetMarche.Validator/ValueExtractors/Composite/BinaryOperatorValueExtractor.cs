using System;
using System.Collections.Generic;
using System.Text;
using DotNetMarche.Validator.Interfaces;
using System.Reflection;

namespace DotNetMarche.Validator.ValueExtractors.Composite
{
	/// <summary>
	/// 
	/// </summary>
	public class BinaryOperatorValueExtractor :IValueExtractor {

		public enum MathOperation {
			Multiplication,
			Addition,
			Division,
			Subtraction,
		}
		
		public BinaryOperatorValueExtractor(
			IValueExtractor left,
			IValueExtractor right,
			MathOperation	 operation) {
		
			mLeft				= left;
			mRight			= right;
			mOperation		= operation;
			}

		IValueExtractor	mLeft;
		IValueExtractor	mRight;
		MathOperation		mOperation;

		public object ExtractValue(object objToValidate) {
			
			Double left = Convert.ToDouble(mLeft.ExtractValue(objToValidate));
			Double right =  Convert.ToDouble(mRight.ExtractValue(objToValidate));
			
			switch (mOperation) {
				case MathOperation.Addition:
					return left + right;
				case MathOperation.Division:
					return left / right;
				case MathOperation.Multiplication:
					return left * right;
				case MathOperation.Subtraction:
					return left - right;
				default:
					throw new ArgumentException("Unknown operation");
			}
		}

		#region IValueExtractor Members


		public string SourceName
		{
			get { return mLeft.SourceName + GetSymbolicValue() + mRight.SourceName; }
		}

		private string GetSymbolicValue()
		{
			switch (mOperation)
			{
				case MathOperation.Addition:
					return " + ";
				case MathOperation.Division:
					return " / ";
				case MathOperation.Multiplication:
					return " * ";
				case MathOperation.Subtraction:
					return " - ";
				default:
					throw new ArgumentException("Unknown operation");
			}
		}

		#endregion
	}
}