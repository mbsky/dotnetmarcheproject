using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNetMarche.Infrastructure.Helpers
{
	/// <summary>
	/// Helps the generation of the id for the InMemoryRegistry
	/// </summary>
	public abstract class IdGenerator
	{
		/// <summary>
		/// This is the only function, a function that generate new id every time
		/// the caller calls for a new id.
		/// </summary>
		/// <returns></returns>
		public abstract Object Generate();
	}

	/// <summary>
	/// A generator of Id based on sequence that supports only integer number.
	/// </summary>
	public class SequenceIdGenerator : IdGenerator
	{
		private Int32 CurrentValue = 0;

      public override object Generate()
		{
      	return Interlocked.Increment(ref CurrentValue);
		}
	}
}
