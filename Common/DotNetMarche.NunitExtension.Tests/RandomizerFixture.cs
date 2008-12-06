using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.NunitExtension.Attributes;
using NUnit.Framework;

namespace DotNetMarche.NunitExtension.Tests
{
	[RandomizeTestOrderFixture]
	public class RandomizerFixture
	{

		[Test]
		public void TestA()
		{
			Console.WriteLine("TESTA");
		}

		[Test]
		public void TestB()
		{
			Console.WriteLine("TESTB");
		}

		[Test]
		public void TestC()
		{
			Console.WriteLine("TESTC");
		}

		[Test]
		public void TestD()
		{
			Console.WriteLine("TESTD");
		}
	}
}
