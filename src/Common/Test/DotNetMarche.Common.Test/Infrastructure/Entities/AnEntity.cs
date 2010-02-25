using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.TestHelpers;

namespace DotNetMarche.Common.Test.Infrastructure.Entities
{
	internal class AnEntity
	{
		private Int32 Id { get;  set; }
		public String Name { get; set; }
		public Int32 Value { get; set; }

		public Int32 fieldValue;

		public static AnEntity Create(Int32 id, String name, Int32 value)
		{
			return new AnEntity() { Id = id, Name = name, Value = value, fieldValue = value};
		}

		public static AnEntity Create(AnEntity original)
		{
			return new AnEntity() { Id = original.Id, Name = original.Name, Value = original.Value, fieldValue = original.fieldValue };
		}

		public static AnEntity CreateSome()
		{
			return new AnEntity() { Name = Generator.RandomStringUnique(10), Value = 99, fieldValue =99 };
		}

		public static AnEntity CreateWithIdOnly(Int32 id)
		{
			return new AnEntity() { Id = id};
		}
	}

	internal class AnotherEntity
	{
		private Int32 Id { get; set; }
		public String Name { get; set; }
		public AnEntity Entity { get; set; }

		public AnEntity fieldEntity;

	}

	internal class AnEntityWithCollection
	{
		public Int32 Id { get; set; }
		public ICollection<String> Collection { get; set; }

		public AnEntityWithCollection()
		{
			Collection = new System.Collections.Generic.LinkedList<String>();
		}
	}

	internal class AnEntityWithComplexCollection
	{
		public Int32 Id { get; set; }
		public ICollection<AnEntity> Collection { get; set; }

		public AnEntityWithComplexCollection()
		{
			Collection = new System.Collections.Generic.LinkedList<AnEntity>();
		}
	}

	internal class AnEntityWithDictionary
	{
		public Int32 Id { get; set; }
		public IDictionary<String, Int32> Dictionary { get; set; }

		public AnEntityWithDictionary()
		{
			Dictionary = new System.Collections.Generic.Dictionary<String, Int32>();
		}
	}
}
