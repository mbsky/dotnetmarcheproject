using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.Infrastructure.Entities.ForEntityIdFinder
{
	public class PublicIdEntity
	{
		public Int32 Id;

		internal static PublicIdEntity Create(Int32 id)
		{
			return new PublicIdEntity() { Id = id };
		}
	}
}
