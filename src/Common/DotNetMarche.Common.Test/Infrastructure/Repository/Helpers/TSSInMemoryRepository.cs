using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Concrete;

namespace DotNetMarche.Common.Test.Infrastructure.Repository.Helpers
{
	/// <summary>
	/// TSS = Test Specific Subclass 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	class TSSInMemoryRepository<T> : InMemoryRepository<T>
	{

		public Dictionary<Object, T> TheContext
		{
			get { return context; }
		}
	}
}
