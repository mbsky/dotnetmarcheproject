using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.Infrastructure.Entities
{
	internal class AnEntity
	{
		private Int32 Id { get; set; }
		public String Name { get; set; }
		public Int32 Value { get; set; }

		public static AnEntity Create(Int32 id, String name, Int32 value)
		{
			return new AnEntity() {Id = id, Name = name, Value = value};
		}

		public static AnEntity CreateWithIdOnly(Int32 id)
		{
			return new AnEntity() { Id = id};
		}
	}
}
