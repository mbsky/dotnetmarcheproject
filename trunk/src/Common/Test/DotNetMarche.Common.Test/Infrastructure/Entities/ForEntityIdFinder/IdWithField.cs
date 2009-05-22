using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.Infrastructure.Entities.ForEntityIdFinder
{
	/// <summary>
	/// To test the 
	/// </summary>
	internal class IdWithField
	{
		private Int32 Id;

		internal static IdWithField Create(Int32 id)
		{
			return new IdWithField() {Id = id};
		}
	}
}