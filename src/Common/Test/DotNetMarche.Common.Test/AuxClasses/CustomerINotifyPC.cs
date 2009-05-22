using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DotNetMarche.Common.Test.AuxClasses
{
	public class CustomerINotifyPC : INotifyPropertyChanged, INotifyPropertyChanging 
	{
		
		public String Name
		{
			get { return name; }
			set
			{
				OnPropertyChanging("Name");
				name = value;
				OnPropertyChanged("Name");
			}
		}
		private String name;
		
		public String Surname
		{
			get { return surname; }
			set
			{
				OnPropertyChanging("Surname");
				surname = value;
				OnPropertyChanged("Surname");
			}
		}
		private String surname;

		
		public Int32 Age
		{
			get { return age; }
			set
			{
				OnPropertyChanging("Age");
				age = value;
				OnPropertyChanged("Age");
			}
		}
		private Int32 age;   
          
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(String propertyName)
		{
			PropertyChangedEventHandler temp = PropertyChanged;
			if (temp != null)
				temp(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region INotifyPropertyChanging Members

		public event PropertyChangingEventHandler PropertyChanging;
		protected void OnPropertyChanging(String propertyName)
		{
			PropertyChangingEventHandler temp = PropertyChanging;
			if (temp != null)
				temp(this, new PropertyChangingEventArgs(propertyName));
		}

		#endregion
	}
}
