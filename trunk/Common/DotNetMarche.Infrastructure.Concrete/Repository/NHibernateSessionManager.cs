using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Infrastructure.Base;
using DotNetMarche.Infrastructure.Helpers;
using NHibernate;
using NHibernate.Cfg;
using DotNetMarche.Utils;
using NHibernate.Tool.hbm2ddl;

namespace DotNetMarche.Infrastructure.Concrete.Repository
{

	/// <summary>
	/// Manage nhibernate session lifecycle, this component work in a 
	/// simple fashion, you ask for a session for the first time, the 
	/// session will be stored in context, then until you does not 
	/// release the session, you continue to use the same session.
	/// </summary>
	public static class NHibernateSessionManager
	{
		private const String ContextSessionKey = "0C815000-9CC1-45c8-A143-71975F9F896B";
		private static String GetContextSessionKeyForConfigFileName(String filename)
		{
			return ContextSessionKey + filename;
		}

		private class NhibConfigData
		{
			public ISessionFactory SessionFactory { get; set; }
			public NHibernate.Cfg.Configuration Configuration { get; set; }
		}

		private static Dictionary<String, NhibConfigData> factories = new Dictionary<String, NhibConfigData>();

		public static ISession GetSession()
		{
			return GetSessionFor("hibernate.cfg.xml");
		}

		public static ISession GetSessionFor(String configFileName)
		{
			Object obj = CurrentContext.GetData(GetContextSessionKeyForConfigFileName(configFileName));
			if (null == obj)
			{
				NhibConfigData configData = GetOrCreateConfigData(configFileName);
				ISession session = configData.SessionFactory.OpenSession();
				CurrentContext.SetData(GetContextSessionKeyForConfigFileName(configFileName), session);
				return session;
			}
			return (ISession)obj;
		}

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

		/// <summary>
		/// Close the open session corresponding to the current config file name. This method 
		/// is responsible to close and dispose the session.
		/// </summary>
		/// <param name="configFileName"></param>
		public static void CloseSessionFor(string configFileName)
		{
			using (ISession session = (ISession)CurrentContext.GetData(GetContextSessionKeyForConfigFileName(configFileName)))
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
			foreach(KeyValuePair<String, Object> kvp in CurrentContext.Enumerate().SafeEnumerate())
			{
				if (kvp.Key.StartsWith(ContextSessionKey))
				{
					//Yes this is a session.
					CloseSessionFor(kvp.Key.Substring(ContextSessionKey.Length));
				}
			}
		}

		private static NhibConfigData GetOrCreateConfigData(String configFileName)
		{
			NhibConfigData retvalue = factories.SafeGet(configFileName);
			if (null == retvalue)
			{
				//This is the first time we ask for this configuration
				NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();
				config.Configure(configFileName);
				ISessionFactory factory = config.BuildSessionFactory();
				retvalue = new NhibConfigData() { Configuration = config, SessionFactory = factory };
				factories.Add(configFileName, retvalue);
			}
			return retvalue;
		}



	}
}
