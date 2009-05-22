using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Helpers;

namespace DotNetMarche.Infrastructure.UI
{
	/// <summary>
	/// This is the undoRedo wrapper to handle undo and redo of 
	/// objects that implements INotifyPropertyChanging and INotifyPropertyChanged.
	/// TODO: Make a faster implementation avoiding continuos reflection 
	/// </summary>
	public class UndoRedoINPC : UndoRedoList
	{
		private Object controlledObject;
		public UndoRedoINPC(INotifyPropertyChanged obj)
		{
			Verify.That(obj is INotifyPropertyChanging,
			            "The object should implement both INotifyPropertyChanged and INotifyPropertyChanging");
			obj.PropertyChanged += obj_PropertyChanged;
			((INotifyPropertyChanging)obj).PropertyChanging += UndoRedoINPC_PropertyChanging;
			controlledObject = obj;
		}

		private Object originalValue;
		private Boolean isChangingInternally = false;

		void UndoRedoINPC_PropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (isChangingInternally) return;
			originalValue = GetValue(e.PropertyName);
		}

		private void obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (isChangingInternally) return;
			Object beforeChangeValue = originalValue;
			originalValue = null;
			Object actualValue = GetValue(e.PropertyName);
			Action undo = () => SetValue(e.PropertyName, beforeChangeValue);
			Action redo = () => SetValue(e.PropertyName, actualValue);
			base.Add(String.Format("Property {0} changed from [{1}] to [{2}]", e.PropertyName, beforeChangeValue, actualValue), undo, redo);
		}

		private Object GetValue(String propertyName)
		{
			return controlledObject.GetType().GetProperty(propertyName).GetValue(controlledObject, new Object[] { });
		}		
		
		private void SetValue(String propertyName, Object value)
		{
			 controlledObject.GetType().GetProperty(propertyName).SetValue(controlledObject, value, new Object[] { });
		}

		public override void Undo()
		{
			isChangingInternally = true;
			base.Undo();
			isChangingInternally = false;
		}

		public override void Redo()
		{
			isChangingInternally = true;
			base.Redo();
			isChangingInternally = false;
		}
	}
}
