using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Concrete;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace DotNetMarche.Common.Test.Infrastructure
{
	[TestFixture]
	public class ContextTest
	{
		[Test]
		public void ReturnNothingIfKeyNotPresent()
		{
			RemotingCallContext sut = new RemotingCallContext();
			Assert.That(sut.GetData("unknownkey"), Is.Null);
		}
	}
}
