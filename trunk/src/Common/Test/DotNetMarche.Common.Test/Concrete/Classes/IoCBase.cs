using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.Concrete.Classes
{
	public interface IIoCBase
	{
		Int32 Value { get; }
	}

	public interface IIoCChain
	{
		IoCBase TheBase { get; }
	}

	public class IoCBase : IIoCBase
	{
		public String Property { get; set; }
		public Int32 Value { get; set; }
		public IoCBase(Int32 needValue)
		{
			Value = needValue;
		}
	}

	public class IoCChain : IIoCChain
	{
		public IoCBase TheBase { get; set; }

		public IoCChain(IoCBase theBase)
		{
			TheBase = theBase;
		}
	}
}
