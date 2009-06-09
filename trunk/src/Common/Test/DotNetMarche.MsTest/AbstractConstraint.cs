using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest
{
	/// <summary>
	/// This is the basic constratint for having fluent assertion for msTest
	/// </summary>
	public abstract class AbstractConstraint : IConstraint
	{
		#region IConstraint Members

		public bool Validate(object subject)
		{
			return InnerValidate(subject);
		}

		#endregion

		internal abstract Boolean InnerValidate(Object subject);

	}
}
