using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.Infrastructure.Entities.ForEntityIdFinder
{
	internal class IdNotNamedId
	{
		internal Int32 MyId { get; set; }

		internal static IdNotNamedId Create()
		{
			return new IdNotNamedId();
		}
	}
}