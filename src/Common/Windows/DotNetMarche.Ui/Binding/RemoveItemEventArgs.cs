using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.Ui.Binding
{
	public class RemoveItemEventArgs : EventArgs
	{
		public Object RemovedItem
		{
			get { return removedItem; }
		}
		private Object removedItem;

		public RemoveItemEventArgs(object removedItem)
		{
			this.removedItem = removedItem;
		}
	}
}
