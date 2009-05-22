using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Concrete;
using DotNetMarche.TestHelpers.SyntaxHelpers;
using DotNetMarche.TestHelpers.Threading;
using NUnit.Framework;

namespace DotNetMarche.Common.Test.Concrete
{
	[TestFixture]
	public class SomeTests
	{
		private ParallelFunctionTester tester = new ParallelFunctionTester();

		[Test]
		public void TestBasicRemotingContext()
		{
			RemotingCallContext context = new RemotingCallContext();
			tester.CallMultiple((o, cs) =>
			                    	{
			                    		context.SetData("Test", o);
			                    		cs.Switch();
			                    		Assert.That(context.GetData("Test"), Is.EqualTo(o));
			                    	}, 3, new Object[] {"A", "B", "C"});
			tester.AssertAll();
		}

	}
}
