using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Infrastructure.Constraint
{
	/// <summary>
	/// This is the generic interface of a constraint, it can be used to assert 
	/// something on some object, this can be useful for 
	/// </summary>
	public interface IConstraint
	{
		/// <summary>
		/// Verify that the object matched the constraint.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		Boolean Match(Object obj);


	}
}
