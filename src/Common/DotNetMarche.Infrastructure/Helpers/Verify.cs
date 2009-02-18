using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Exceptions;

namespace DotNetMarche.Infrastructure.Helpers
{
	public static class Verify
	{
		public static void That(Boolean condition, String errorMessages)
		{
			if (!condition)
				throw new VerificationFailedException(errorMessages);
		}

		public static class Argument
		{
			public static void For(Boolean condition, String argumentName, String errorMessages)
			{
				if (!condition)
					throw new ArgumentException(errorMessages, argumentName);
			}
		}

	}
}
