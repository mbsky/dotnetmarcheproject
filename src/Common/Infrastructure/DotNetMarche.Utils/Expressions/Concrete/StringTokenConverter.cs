using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Utils.Expressions.Concrete
{
	public class StringTokenConverter : ITokenConverter<String, String>
	{
		#region ITokenConverter<string,string> Members

		public string Convert(String source)
		{
			return source;
		}

		#endregion
	}
}
