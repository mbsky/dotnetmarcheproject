using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.Utils.Expressions.AuxClasses;
using DotNetMarche.TestHelpers.BaseTests;
using DotNetMarche.Ui.Binding;
using NUnit.Framework;

using Rhino.Mocks;
using RhinoIs = Rhino.Mocks.Constraints.Is;
namespace DotNetMarche.Common.Test.Ui
{
	[TestFixture]
	public class BindingListTest : BaseTestFixture
	{
		#region Initialization

		private Customer[] customerList = new Customer[]
		                                  	{
		                                  		new Customer() {Name = "Guardian", Age=33},
		                                  		new Customer() {Name = "Alkampfer", Age=34},
		                                  		new Customer() {Name = "DiegoGuidi", Age=22},
		                                  		new Customer() {Name = "Gnosi", Age=100},
		                                  	};

		private MockRepository mockRepository = new MockRepository();

		public interface IBindingListEventSink
		{
			void HandleRemoved(Object sender, RemoveItemEventArgs e);
			void HandleAdded(Object sender, AddingNewEventArgs e);
		}

		protected override void OnSetUp()
		{
			mockRepository = new MockRepository();
			ExecuteAtTheEndOfTest(() => mockRepository.VerifyAll());
		}

		#endregion

		[Test]
		public void BasicTestForFilter()
		{
			BindingListExt<Customer> sut = CreateBindingListOnBasicCustomersList();
			sut.Filter = "Name == 'Alkampfer'";
            Assert.That(sut, Has.Count.EqualTo(1));
		}

		[Test]
		public void BasicTestForFilterAndRemove()
		{
			BindingListExt<Customer> sut = CreateBindingListOnBasicCustomersList();
			sut.Filter = "Name == 'Alkampfer'";
			sut.Filter = String.Empty;
			Assert.That(sut, Has.Count.EqualTo(4));
		}

		[Test]
		public void FilterAddElementThenRemoveFilter()
		{
			BindingListExt<Customer> sut = CreateBindingListOnBasicCustomersList();
			sut.Filter = "Name == 'Alkampfer'";
			sut.Add(new Customer() {Name = "Mark Fields", Age = 28});
			sut.Filter = String.Empty;
            Assert.That(sut, Has.Count.EqualTo(5));
		}

		[Test]
		public void TestThatAddedElementDoesNotGoIntoTheFilter()
		{
			BindingListExt<Customer> sut = CreateBindingListOnBasicCustomersList();
			sut.Filter = "Name == 'Alkampfer'";
			sut.Add(new Customer() { Name = "Mark Fields", Age = 28 });
            Assert.That(sut, Has.Count.EqualTo(1));
		}

		[Test]
		public void TestBasicFilterDoesNotRaiseRemove()
		{
			BindingListExt<Customer> sut = CreateBindingListOnBasicCustomersList();
			IBindingListEventSink mock = mockRepository.CreateMock<IBindingListEventSink>();
			sut.RemovingItem += mock.HandleRemoved;
			Expect.Call(() => mock.HandleRemoved(null, null)).Repeat.Never();
			mockRepository.ReplayAll();
			sut.Filter = "Name == 'Alkampfer'";
		}

		[Test]
		public void TestBasicFilterDoesNotRaiseAdd()
		{
			BindingListExt<Customer> sut = CreateBindingListOnBasicCustomersList();
			IBindingListEventSink mock = mockRepository.CreateMock<IBindingListEventSink>();
			sut.AddingNew += mock.HandleAdded;
			Expect.Call(() => mock.HandleAdded(null, null)).Repeat.Never();
			mockRepository.ReplayAll();
			sut.Filter = "Name == 'Alkampfer'";
			sut.Filter = "";
		}

		[Test]
		public void TestThatAddedElementAlwaysRaiseEvent()
		{
			BindingListExt<Customer> sut = CreateBindingListOnBasicCustomersList();
			IBindingListEventSink mock = mockRepository.CreateMock<IBindingListEventSink>();
			sut.AddingNew += mock.HandleAdded;
			Customer cust = new Customer() {Name = "Mark Fields", Age = 28};
			Expect.Call(() => mock.HandleAdded(sut, null))
				.Constraints(RhinoIs.Equal(sut), RhinoIs.Matching<AddingNewEventArgs>(args => args.NewObject == cust))
				.Repeat.Once();
			mockRepository.ReplayAll();
			sut.Filter = "Name == 'Alkampfer'";
			sut.Add(cust);
		}

		#region Factory

		private BindingListExt<Customer> CreateBindingListOnBasicCustomersList()
		{
			BindingListExt<Customer> sut = new BindingListExt<Customer>();
			sut.AddRange(customerList);
			return sut;
		}

		#endregion
	}
}