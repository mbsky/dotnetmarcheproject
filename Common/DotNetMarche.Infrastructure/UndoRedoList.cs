using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Helpers;

namespace DotNetMarche.Infrastructure
{
	public class UndoRedoList
	{
		private class ActionStep
		{
			public String ActionName { get; set; }
			public Action UndoAction { get; set; }
			public Action RedoAction { get; set; }
		}

		private readonly List<ActionStep> steps = new List<ActionStep>();
		private Int32 undoPosition = 0;

		public IEnumerable<String> ActionList
		{
			get { return steps.Select(act => act.ActionName); }
		}

		public Boolean IsRedoActive
		{
			get { return steps.Count - undoPosition > 0; }
		}

		public Boolean IsUndoActive
		{
			get { return undoPosition > 0; }
		}

		public Int32 UndoCount
		{
			get { return undoPosition; }
		}		
		
		public Int32 RedoCount
		{
			get { return steps.Count - undoPosition; }
		}

		public void Add(Action undoAction, Action redoAction)
		{
			Add(String.Empty, undoAction, redoAction);
		}

		public void Add(String actionName, Action undoAction, Action redoAction)
		{
			if (undoPosition  < steps.Count)
				steps.RemoveRange(undoPosition, steps.Count - undoPosition);
			steps.Add(new ActionStep() { ActionName = actionName, UndoAction = undoAction, RedoAction = redoAction });
			undoPosition++;
		}

		public virtual void Redo()
		{
			Verify.That(RedoCount > 0, "Cannot undo any action, the RedoList is empty");
			steps[undoPosition].RedoAction();
			undoPosition++;
		}

		public virtual void Undo()
		{
			Verify.That(UndoCount > 0, "Cannot undo any action, the UndoList is empty");
			undoPosition--;
			steps[undoPosition].UndoAction();

		}
	}
}
