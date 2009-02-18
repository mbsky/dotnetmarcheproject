using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;
using DotNetMarche.Utils;

namespace DotNetMarche.Common.Test.AuxClasses
{
	internal class TestContext : IContext
	{
		internal Dictionary<String, Object> storage = new Dictionary<String, Object>();

		#region IContext Members

		public object GetData(string key)
		{
			return storage.SafeGet(key);
		}

		public void SetData(string key, object value)
		{
			storage[key] = value;
		}

		public void ReleaseData(string key)
		{
			storage.Remove(key);
		}

		public object this[string key]
		{
			get
			{
				return GetData(key);
			}
			set
			{
				SetData(key, value);
			}
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,object>> Members

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return storage.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
