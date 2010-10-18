using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.Utils.Expressions.AuxClasses
{
	class Customer
	{
		public String Name { get; set; }
		public Int32 Age { get; set; }
		public Address Address { get; set; }
	}

	class Address
	{
		public Int32 Number { get; set; }
		public String Street { get; set; }
	}
}
