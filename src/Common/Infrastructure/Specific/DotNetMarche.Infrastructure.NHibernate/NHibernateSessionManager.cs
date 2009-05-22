using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Data;
using DotNetMarche.Infrastructure.Helpers;
using NHibernate;
using NHibernate.Cfg;
using DotNetMarche.Utils;
using NHibernate.Tool.hbm2ddl;
using System.Xml.Linq;
using DotNetMarche.Utils;

namespace DotNetMarche.Infrastructure.NHibernate
{
	/// <summary>
	/// Manage nhibernate session lifecycle, this component work in a 
	/// simple fashion, you ask for a session for the first time, the 
	/// session will be stored in context, then until you does not 
	/// release the session, you continue to use the same session.
	/// </summary>
	public static class NHibernateSessionManager
	{
		#region Internal classes

		private class NhibConfigData
		{
			public ISessionFactory SessionFactory { get; set; }
			public global::NHibernate.Cfg.Configuration Configuration { get; set; }
			public String ConnectionName;
		}

		private class SessionData
		{
			public ISession Session;
			public String connectionName;
		}

		#endregion

		#region Inner variables

		private const String ContextSessionKey = "0C815000-9CC1-45c8-A143-71975F9F896B";
		private static String GetContextSessionKeyForConfigFileName(String filename)
		{
			return ContextSessionKey + filename;
		}

		private static NhibConfigData GetOrCreateConfigData(String configFileName)
		{
			NhibConfigData retvalue = factories.SafeGet(configFileName);
			if (null == retvalue)
			{
				//This is the first time we ask for this configuration
				global::NHibernate.Cfg.Configuration config = new global::NHibernate.Cfg.Configuration();
				XDocument doc = XDocument.Load(configFileName);
				XElement connStringElement = (from e in doc.Descendants()
														where e.Attribute("name") != null && e.Attribute("name").Value == "connection.connection_string"
														select e).Single();
				String cnName = connStringElement.Value;
				connStringElement.Value = ConfigurationRegistry.ConnectionString(connStringElement.Value).ConnectionString;
				using (XmlReader reader = doc.CreateReader())
				{
					config.Configure(reader);
				}
				ISessionFactory factory = config.BuildSessionFactory();
				retvalue = new NhibConfigData() { Configuration = config, SessionFactory = factory, ConnectionName = cnName };
				factories.Add(configFileName, retvalue);
			}
			return retvalue;
		}

		private static Dictionary<String, NhibConfigData> factories = new Dictionary<String, NhibConfigData>();

		#endregion

		#region Initialization

		static NHibernateSessionManager()
		{
			GlobalTransactionManager.TransactionOpened += OnTransactionStarted;
			GlobalTransactionManager.TransactionClosing += OnTransactionClosing;
		}

		/// <summary>
		/// When a transaction start all session must be disconnected from the current connection
		/// and reconnect to a different connection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnTransactionStarted(Object sender, EventArgs e)
		{
			//if we are in a nested transaction we do not need to do nothing, we are working simply
			//with more transaction level.
			if (GlobalTransactionManager.TransactionsCount > 1) return;
			//This is the first time that a transaction start, we need to change all connection with those
			//of the DataAccess layer.
			IterateThroughAllOpenSessionInContext(sd =>
			{
				DataAccess.ConnectionData data = DataAccess.GetActualConnectionData(sd.connectionName);
				sd.Session.Disconnect();
				sd.Session.Reconnect(data.Connection);
			});
		}

		/// <summary>
		/// Monitor when a global transaction close, we need to disconnect from the current transaction
		/// and reconnect.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void OnTransactionClosing(Object sender, TransactionClosingEventArgs e)
		{
			//First of all check if the transaction is rollbacked, this means that we need to dispose all
			//Sessions because all sessions are no longer usable.
			if (e.IsDoomed)
			{
				IterateThroughAllOpenSessionInContext(sd => sd.Session.Dispose());
			}
			else
			{
				//a transaction is about to be committed, flush everything
				IterateThroughAllOpenSessionInContext(sd => sd.Session.Flush());
				
				//if we  are still in a global transaction, we can still use the
				//same connection because DataAccess layer use the same connection for all transaction level.
				if (GlobalTransactionManager.TransactionsCount > 2) return;

				//Ok, we are out of all transaction, we need to recreate a valid connection for each session.
				IterateThroughAllOpenSessionInContext(sd =>
				{
					sd.Session.Disconnect();
					sd.Session.Reconnect();
				});
			}
		}

		#endregion

		#region Session Management

		public static ISession GetSession()
		{
			return GetSessionFor("hibernate.cfg.xml");
		}

		public static ISession GetSessionFor(String configFileName)
		{
			Object obj = CurrentContext.GetData(GetContextSessionKeyForConfigFileName(configFileName));
			if (null != obj)
			{
				//We have a session on the stack, but, it is valid?
				ISession contextSession = ((SessionData)obj).Session;
				if (contextSession.IsOpen) return contextSession;
				//Session is not valid, remove from the context.
				CurrentContext.ReleaseData(GetContextSessionKeyForConfigFileName(configFileName));
			}
			//If we reach here we have no context connection or the context connection was disposed.
			NhibConfigData configData = GetOrCreateConfigData(configFileName);

			ISession session = null;
			if (GlobalTransactionManager.IsInTransaction)
			{
				DataAccess.ConnectionData data = DataAccess.GetActualConnectionData(configData.ConnectionName);
				session = configData.SessionFactory.OpenSession(data.Connection);
			}
			else
			{
				session = configData.SessionFactory.OpenSession();
			}
			CurrentContext.SetData(
				GetContextSessionKeyForConfigFileName(configFileName),
				new SessionData() { Session = session, connectionName = configData.ConnectionName });
			return session;
		}

		/// <summary>
		/// Close the open session corresponding to the current config file name. This method 
		/// is responsible to close and dispose the session.
		/// </summary>
		/// <param name="configFileName"></param>
		public static void CloseSessionFor(string configFileName)
		{
			SessionData sd = (SessionData)CurrentContext.GetData(GetContextSessionKeyForConfigFileName(configFileName));
			using (ISession session = sd.Session)
			{
				session.Flush();
				CurrentContext.ReleaseData(GetContextSessionKeyForConfigFileName(configFileName));
			}
		}

		/// <summary>
		/// Close all open session
		/// </summary>
		public static void CloseSessions()
		{
			foreach (KeyValuePair<String, Object> kvp in CurrentContext.Enumerate().SafeEnumerate())
			{
				if (kvp.Key.StartsWith(ContextSessionKey))
				{
					//Yes this is a session.
					CloseSessionFor(kvp.Key.Substring(ContextSessionKey.Length));
				}
			}
		}

		/// <summary>
		/// Helper function that permits to iterate into all opened sessions. This management
		/// function is useful because it does not invoke the callback for session that are
		/// closed and still in the Context.
		/// </summary>
		/// <param name="action"></param>
		private static void IterateThroughAllOpenSessionInContext(Action<SessionData> action)
		{
			var openSessions = (from ck in CurrentContext.Enumerate()
									  where ck.Key.StartsWith(ContextSessionKey)
									  select ck.Value).Cast<SessionData>();
			openSessions.Where(sd => sd.Session.IsOpen).ToList().ForEach(action);
		}

		#endregion

		#region Schema Management

		/// <summary>
		/// Generate the database for the configuration required.
		/// </summary>
		/// <param name="configFileName"></param>
		public static void GenerateDbFor(String configFileName)
		{
			NhibConfigData configData = GetOrCreateConfigData(configFileName);
			SchemaExport se = new SchemaExport(configData.Configuration);
			se.Create(false, true);
		}

		#endregion
	}
}