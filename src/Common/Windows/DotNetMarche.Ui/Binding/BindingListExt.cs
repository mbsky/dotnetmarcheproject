﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Helpers;
using DotNetMarche.Utils.Linq;

namespace DotNetMarche.Ui.Binding
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;

	public class BindingListExt<T> : BindingList<T>, IBindingListView
	{

		#region Aux functions

		public void AddRange(IEnumerable<T> listToAdd)
		{
			foreach (T element in listToAdd)
			{
				this.Add(element);
			}
		}

		#endregion

		#region Find and searching

		protected override bool SupportsSearchingCore
		{
			get
			{
				return true;
			}
		}

		protected override int FindCore(PropertyDescriptor prop, object key)
		{
			if (key is T)
			{
				GenericIComparer<T> comparer = GenericComparerFactory.GetComparer<T>(prop.Name, false);
				for (Int32 index = 0; index < Items.Count; ++index)
				{
					if (comparer.Compare(Items[index], (T)key) == 0)
						return index;
				}
				return -1;
			}
			throw new NotSupportedException("Cannot compare Apple With Orange");
		}

		#endregion

		#region Additional events

		public event EventHandler<RemoveItemEventArgs> RemovingItem;
		protected virtual void OnRemovingItem(RemoveItemEventArgs args)
		{
			EventHandler<RemoveItemEventArgs> temp = RemovingItem;
			if (temp != null)
			{
				temp(this, args);
			}

		}

		protected override void RemoveItem(int index)
		{
			OnRemovingItem(new RemoveItemEventArgs(this[index]));
			if (!String.IsNullOrEmpty(filterClause))
			{
				original.RemoveAt(index);
			}
			base.RemoveItem(index);
		}

		#endregion

		#region Constructors

		public BindingListExt(IList<T> list)
			: base(list)
		{
		}

		public BindingListExt()
		{
		}

		#endregion

		#region Support sorting and filtering

		private Func<T, Boolean> filterPredicate;

		private List<T> original = new List<T>();

		private void DoFilter()
		{
			if (String.IsNullOrEmpty(filterClause))
			{
				//Empty filter, return to original content.
				Items.Clear();
				original.ForEach(e => Items.Add(e));
			}
			else
			{
				//create a filter, save everything in the original list.
				original.Clear();
				original.AddRange(Items);
				//Now move in the real data all the element that satisfy the filter.
				Items.Clear();
				filterPredicate = DynamicLinq.ParseToFunction<T, Boolean>(filterClause);
				foreach (T element in original.Where(filterPredicate))
				   Items.Add(element);
			}
			InnerPerformSort();
		}

		/// <summary>
		/// è necessario intercettare l'isnerimento, questo perchè se inserisco
		/// durante un filtraggio è necessario inserire ilt utto anche nella lista
		/// originale.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		protected override void InsertItem(int index, T item)
		{
			if (!String.IsNullOrEmpty(filterClause))
			{
				//Ho un filtro attivo nella lista originale ci va sempre
				original.Insert(index, item);
				OnAddingNew(new AddingNewEventArgs(item));
				if (filterPredicate(item)) base.InsertItem(index, item);
				return;
			}
			//If it respect the filter then add to the filtered collction
			base.InsertItem(index, item);
			InnerPerformSort();
		}

		#endregion

		#region IBindingListView Members

		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			throw new NotImplementedException();
		}

		public string Filter
		{
			get
			{
				return filterClause;
			}
			set
			{
				if (filterClause == value) return;
				filterClause = value;
				DoFilter();
			}
		}



		private string filterClause = String.Empty;

		public void RemoveFilter()
		{
			Filter = String.Empty;
		}

		public ListSortDescriptionCollection SortDescriptions
		{
			get { throw new NotImplementedException(); }
		}


		public bool SupportsAdvancedSorting
		{
			get { return false; }
		}

		public bool SupportsFiltering
		{
			get { return true; }
		}

		#endregion

		#region SortingBase

		protected override bool SupportsSortingCore
		{
			get
			{
				return true;
			}
		}

		private PropertyDescriptor _currentSortPropertyDescriptor = null;

		private ListSortDirection _currentSortDirection;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="direction"></param>
		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			_currentSortPropertyDescriptor = prop;
			_currentSortDirection = direction;
			InnerPerformSort();
		}

		private void InnerPerformSort()
		{
			if (_currentSortPropertyDescriptor == null) return;
			List<T> temp = Items.ToList();
			Items.Clear();
			GenericIComparer<T> cmp = GenericComparerFactory.GetComparer<T>(_currentSortPropertyDescriptor.Name, _currentSortDirection == ListSortDirection.Descending);
			temp.Sort(cmp);
			foreach (T element in temp)
			{
				Items.Add(element);
			}
		}

		#endregion
	}
}
