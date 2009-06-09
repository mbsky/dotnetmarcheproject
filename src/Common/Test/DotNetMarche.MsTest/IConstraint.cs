using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.MsTest
{
	public interface IConstraint
	{
		Boolean Validate(Object subject);

	}
}
