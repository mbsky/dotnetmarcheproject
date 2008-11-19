﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.Utils.Expressions.AuxClasses;
using DotNetMarche.TestHelpers.SyntaxHelpers;
using NUnit.Framework;
using DotNetMarche.Utils.Linq;

namespace DotNetMarche.Common.Test.Utils.Linq
{
	[TestFixture]
	public class DynamicLinqTest
	{
		private Customer[] customerList = new Customer[]
		{
			new Customer() {Name = "Guardian", Age=33},
			new Customer() {Name = "Alkampfer", Age=34},
			new Customer() {Name = "DiegoGuidi", Age=22},
			new Customer() {Name = "Gnosi", Age=100},
		};

		[Test]
		public void TestWhere()
		{
			IEnumerable<Customer> result = customerList.Where("Age > 30");
			CollectionAssert.Contains(result, customerList[0]);
			CollectionAssert.Contains(result, customerList[1]);
			CollectionAssert.Contains(result, customerList[3]);
			Assert.That(result.Count(), Is.EqualTo(3));
		}

		[Test]
		public void TestWhere2()
		{
			IEnumerable<Customer> result = customerList.Where("Age < 30");
			CollectionAssert.Contains(result, customerList[2]);
			Assert.That(result.Count(), Is.EqualTo(1));
		}		
		
		//[Test]
		//public void TestWhere2F()
		//{
		//   var result = from Customer c in customerList
		//                where "Age < 30"
		//                select c;
		//   CollectionAssert.Contains(result, customerList[2]);
		//   Assert.That(result.Count(), Is.EqualTo(1));
		//}

	}
}
