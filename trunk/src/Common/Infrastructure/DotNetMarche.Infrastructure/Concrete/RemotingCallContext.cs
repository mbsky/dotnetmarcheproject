using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using DotNetMarche.Infrastructure.Base.Interfaces;

namespace DotNetMarche.Infrastructure.Concrete
{
	public class RemotingCallContext : IContext
	{
		private readonly String keyListId = "8EFB080E-36AF-4757-BF77-FC41B1AE566E";
		private void AddKey(String key)
		{
			Object currentkeys = GetData(keyListId);
			if (null == currentkeys )
			{
				currentkeys = new List<String>();
				SetData(keyListId, currentkeys);
			}
			if (!keyListId.Contains(key))
				((List<String>) currentkeys).Add(key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		private void RemoveKey(String key)
		{
			List<String> currentkeys = (List<String>)GetData(keyListId);
			currentkeys.Remove(key);
		}

		#region IContext Members

		public object GetData(string key)
		{
			return CallContext.GetData(key);
		}

		public void ReleaseData(String key)
		{
			CallContext.FreeNamedDataSlot(key);
			RemoveKey(key);
		}

		public void SetData(string key, object value)
		{
			CallContext.SetData(key, value);
			AddKey(key);
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
			Object currentkeys = GetData(keyListId);
			if (null != currentkeys)
			{
				List<String> keylist = (List<String>) currentkeys;
				foreach (String key in keylist)
					yield return new KeyValuePair<String, Object>(key, GetData(key));
			}
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
