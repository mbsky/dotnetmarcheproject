using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DotNetMarche.Validator.Utils;

namespace DotNetMarche.Validator.Tests.Utils
{
	[TestFixture]
	public class CollectionHelperUtilsFixture
	{
		[Test]
		public void VerifyBasicIListInheritFrom()
		{
			
			Type type = typeof (List<Int32>);
			Assert.That(CollectionHelper.IsGenericSubclassOf(type, typeof(List<>)), Is.True);
		}

		[Test]
		public void VerifyBasicIListInheritFromValidateEvenInterfaces()
		{

			Type type = typeof(List<Int32>);
			Assert.That(CollectionHelper.IsGenericSubclassOf(type, typeof(IList<>)), Is.True);
		}		
		
		[Test]
		public void VerifyTypeImplementsGenericInterface()
		{

			Type type = typeof(List<Int32>);
			Assert.That(type.ImplementsInterface(typeof(IList<>)), Is.True);
		}

		[Test]
		public void VerifyTypeImplementsGenericInterfaceIEnumerator()
		{
			List<Int32> obj = new List<int>();
			Type type = obj.GetType();
			Assert.That(type.ImplementsInterface(typeof(IEnumerable)), Is.True);
		}

		[Test]
		public void VerifyRetrievalOfGenericTypeOfAGenericInterface()
		{
			Type type = typeof(List<Int32>);
			Assert.That(type.GetGenericTypeOfIEnumerable(), Is.EqualTo(typeof(Int32)));
		}
	}
}
