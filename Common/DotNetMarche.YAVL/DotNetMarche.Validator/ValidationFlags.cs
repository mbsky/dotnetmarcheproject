using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetMarche.Validator
{
	[Flags()]
	public enum ValidationFlags {
		StopOnFirstError		= 1,
		RecursiveValidation	= 2
	}

	internal class ValdationFlagsUtils {
		internal static Boolean StopOnFirstError(
			ValidationFlags value) {
			return (value & ValidationFlags.StopOnFirstError) == ValidationFlags.StopOnFirstError;  
			}

		internal static Boolean RecursiveValidation(
			ValidationFlags value) {
			return (value & ValidationFlags.RecursiveValidation) == ValidationFlags.RecursiveValidation;  
			}
	}
}