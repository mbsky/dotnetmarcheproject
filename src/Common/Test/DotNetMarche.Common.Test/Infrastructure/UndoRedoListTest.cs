using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Common.Test.AuxClasses;
using DotNetMarche.Infrastructure;
using DotNetMarche.Infrastructure.UI;
using DotNetMarche.TestHelpers.BaseTests;
using NUnit.Framework;

using Rhino.Mocks;

namespace DotNetMarche.Common.Test.Infrastructure
{
	[TestFixture]
	public class UndoRedoListTest : BaseUtilityTest 
	{

		private MockRepository mockRepo;
		protected override  void OnSetUp()
		{
			mockRepo = new MockRepository();
			ExecuteAtTheEndOfTest(mockRepo.VerifyAll);
			base.OnSetUp();
		}

		#region Test Basic

		[Test]
		public void TestBasicListAction()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(null, null);
			Assert.That(sut.UndoCount, Is.EqualTo(1));
		}		
		
		[Test]
		public void TestBasicCannotRedoOrUndo()
		{
			UndoRedoList sut = new UndoRedoList();
			Assert.That(sut.IsRedoActive, Is.False);
			Assert.That(sut.IsUndoActive, Is.False);
		}

		[Test]
		public void TestBasicUndo()
		{
			Action act = mockRepo.CreateMock<Action>();
			act();
			mockRepo.ReplayAll();
			UndoRedoList sut = new UndoRedoList();
			sut.Add(act, null);
			sut.Undo();
		}		
		
		[Test]
		public void TestUndoable()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, null);
			Assert.That(sut.IsUndoActive);
		}

		/// <summary>
		/// There is no redo if we do not undo nothing.
		/// </summary>
		[Test]
		public void TestRedoable()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, NoAction);
			Assert.That(sut.IsRedoActive, Is.False);
		}		
		
		/// <summary>
		/// IF we Undo we should have one redo.
		/// </summary>
		[Test]
		public void TestRedoableWhenUndo()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, NoAction);
			sut.Undo();
			Assert.That(sut.IsRedoActive, Is.True);
		}

		[Test]
		public void TestBasicRedo()
		{
			Action act = mockRepo.CreateMock<Action>();
			act();
			mockRepo.ReplayAll();
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, act);
			sut.Undo();
			sut.Redo();
		}			
		
		/// <summary>
		/// Add two action, then undo, then add another action, the undo list should
		/// prosecute from the actual point
		/// </summary>
		[Test]
		public void TestRedoListCancelled()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, NoAction);
			sut.Add(NoAction, NoAction);
			sut.Undo();
			sut.Add(NoAction, NoAction);
			Assert.That(sut.UndoCount, Is.EqualTo(2));
			Assert.That(sut.RedoCount, Is.EqualTo(0));
		}			
		
		/// <summary>
		/// verify that after an addition of an action the redo is cancelled
		/// </summary>
		[Test]
		public void TestRedoListCancelled2()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, NoAction);
			sut.Add(NoAction, NoAction);
			sut.Add(NoAction, NoAction);
			sut.Undo();
			sut.Add(NoAction, NoAction);
			Assert.That(sut.UndoCount, Is.EqualTo(3));
			Assert.That(sut.RedoCount, Is.EqualTo(0));
		}			
		
		/// <summary>
		/// verify that after an addition of an action the redo is cancelled
		/// </summary>
		[Test]
		public void TestRedoListCancelled3()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, NoAction);
			sut.Add(NoAction, NoAction);
			sut.Add(NoAction, NoAction);
			sut.Undo();
			sut.Undo();
			sut.Add(NoAction, NoAction);
			Assert.That(sut.UndoCount, Is.EqualTo(2));
			Assert.That(sut.RedoCount, Is.EqualTo(0));
		}		
		
		/// <summary>
		/// verify that for each undo we have a redo
		/// </summary>
		[Test]
		public void TestRedoListCount()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, NoAction);
			sut.Add(NoAction, NoAction);
			sut.Add(NoAction, NoAction);
			sut.Undo();
			sut.Undo();
			Assert.That(sut.RedoCount, Is.EqualTo(2));
		}			
		
		[Test]
		public void TestUndoRedoTwoTimes()
		{
			Action undo = mockRepo.CreateMock<Action>();
			Action redo = mockRepo.CreateMock<Action>();
			Expect.Call(() => undo()).Repeat.Twice();
			Expect.Call(() => redo()).Repeat.Twice();
			mockRepo.ReplayAll();
			UndoRedoList sut = new UndoRedoList();
			sut.Add(undo, redo);
			sut.Undo();
			sut.Redo();
			sut.Undo();
			sut.Redo();
		}		
		
		[Test]
		public void TestUndoRedoTwoTimesIsUndoableRedoable()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add(NoAction, NoAction);
			Assert.That(sut.IsUndoActive);
			Assert.That(sut.UndoCount, Is.EqualTo(1));
			sut.Undo();
			Assert.That(sut.IsUndoActive, Is.False);
			Assert.That(sut.UndoCount, Is.EqualTo(0));
			Assert.That(sut.IsRedoActive);
			Assert.That(sut.RedoCount, Is.EqualTo(1));
			sut.Redo();
			Assert.That(sut.IsUndoActive);
			Assert.That(sut.UndoCount, Is.EqualTo(1));
			Assert.That(sut.IsRedoActive, Is.False);
			Assert.That(sut.RedoCount, Is.EqualTo(0));
		}

		#endregion

		#region Test ActionList

		[Test]
		public void TestNameList()
		{
			UndoRedoList sut = new UndoRedoList();
			sut.Add("ActionName", NoAction, NoAction);
			Assert.That(sut.ActionList.Count(), Is.EqualTo(1));
			Assert.That(sut.ActionList.First(), Is.EqualTo("ActionName"));
		}

		#endregion

		[Test]
		public void NPCBaseRedo()
		{
			CustomerINotifyPC obj = new CustomerINotifyPC();
			UndoRedoINPC sut = new UndoRedoINPC(obj);
			obj.Name = "NewValue";
			Assert.That(sut.UndoCount, Is.EqualTo(1));
		}		
		
		[Test]
		public void NPCBaseRedoActionName()
		{
			CustomerINotifyPC obj = new CustomerINotifyPC();
			UndoRedoINPC sut = new UndoRedoINPC(obj);
			obj.Name = "NewValue";
			Assert.That(sut.ActionList.First(), Is.EqualTo("Property Name changed from [] to [NewValue]"));
		}		
		
		[Test]
		public void NPCBaseUndo()
		{
			CustomerINotifyPC obj = new CustomerINotifyPC();
			UndoRedoINPC sut = new UndoRedoINPC(obj);
			obj.Name = "NewValue";
			sut.Undo();
			Assert.That(obj.Name, Is.Null);
		}	
		
		[Test]
		public void NPCBaseUndoTwoLevel()
		{
			CustomerINotifyPC obj = new CustomerINotifyPC();
			UndoRedoINPC sut = new UndoRedoINPC(obj);
			obj.Name = "one";
			obj.Name = "two";
			sut.Undo();
			Assert.That(obj.Name, Is.EqualTo("one"));
			sut.Undo();
			Assert.That(obj.Name, Is.Null);
		}		
		
		[Test]
		public void NPCBaseUndoTwoLevelAndRedo()
		{
			CustomerINotifyPC obj = new CustomerINotifyPC();
			UndoRedoINPC sut = new UndoRedoINPC(obj);
			obj.Name = "one";
			obj.Name = "two";
			sut.Undo();
			Assert.That(obj.Name, Is.EqualTo("one"));
			sut.Undo();
			Assert.That(obj.Name, Is.Null);
			sut.Redo();
			Assert.That(obj.Name, Is.EqualTo("one"));
			sut.Redo();
			Assert.That(obj.Name, Is.EqualTo("two"));
		}

        [Test]
        public void NPCBaseUndoChangeAndRedo()
        {
            CustomerINotifyPC obj = new CustomerINotifyPC();
            UndoRedoINPC sut = new UndoRedoINPC(obj);
            obj.Name = "one";
            obj.Name = "two";
            obj.Name = "three";
            sut.Undo();
            Assert.That(obj.Name, Is.EqualTo("two"));
            sut.Undo();
            Assert.That(obj.Name, Is.EqualTo("one"));
            obj.Name = "four";
            sut.Undo();
            Assert.That(obj.Name, Is.EqualTo("one"));
            sut.Redo();
            Assert.That(obj.Name, Is.EqualTo("four"));
        }

        [Test, ExpectedException(typeof(DotNetMarche.Infrastructure.Exceptions.VerificationFailedException))]
        public void NPCBaseRedoCountLimitExceeded()
        {
            CustomerINotifyPC obj = new CustomerINotifyPC();
            UndoRedoINPC sut = new UndoRedoINPC(obj);
            obj.Name = "one";
            sut.Undo();
            Assert.That(obj.Name, Is.Null);
            sut.Redo();
            Assert.That(obj.Name, Is.EqualTo("one"));
            sut.Redo();
            Assert.That(obj.Name, Is.EqualTo("one"));
        }

        [Test, ExpectedException(typeof(DotNetMarche.Infrastructure.Exceptions.VerificationFailedException))]
        public void NPCBaseUndoCountLimitExceeded()
        {
            CustomerINotifyPC obj = new CustomerINotifyPC();
            UndoRedoINPC sut = new UndoRedoINPC(obj);
            obj.Name = "one";
            sut.Undo();
            Assert.That(obj.Name, Is.Null);
            sut.Undo();
        }
        #region Helpers

		private void NoAction()
		{
		}

		#endregion
	}
}
