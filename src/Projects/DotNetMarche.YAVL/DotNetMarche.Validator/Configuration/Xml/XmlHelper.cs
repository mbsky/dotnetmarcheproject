using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DotNetMarche.Validator.Configuration.Xml
{
	public static class XmlHelper
	{
		/// <summary>
		/// Converte un oggetto in xml grazie alla serializzazione xml
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static String ToXml(Object obj)
		{
			XmlSerializer ser = new XmlSerializer(obj.GetType(), "");
			StringWriter sw = new StringWriter();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.NewLineOnAttributes = true;
			XmlSerializerNamespaces blank = new XmlSerializerNamespaces();
			blank.Add("", "");
			using (XmlWriter writer = XmlWriter.Create(sw, settings))
			{
				ser.Serialize(writer, obj, blank);
			}
			return sw.ToString();
		}

		public static T FromXml<T>(String serializedata)
		{
			XmlSerializer ser = new XmlSerializer(typeof(T), "");

			XmlReaderSettings settings = new XmlReaderSettings();
			XmlSerializerNamespaces blank = new XmlSerializerNamespaces();
			blank.Add("", "");
			using (StringReader sr = new StringReader(serializedata))
			using (XmlReader writer = XmlReader.Create(sr, settings))
			{
				return (T)ser.Deserialize(writer);
			}
		}

		public static Object FromXml(String serializedata, Type[] objectType)
		{
			XmlSerializer ser = new XmlSerializer(objectType[0], objectType.ToArray());

			XmlReaderSettings settings = new XmlReaderSettings();
			XmlSerializerNamespaces blank = new XmlSerializerNamespaces();
			blank.Add("", "");
			using (StringReader sr = new StringReader(serializedata))
			using (XmlReader reader = XmlReader.Create(sr, settings))
			{
				return ser.Deserialize(reader);
			}
		}

		public static Object FromXml(String serializedata, Type objectType)
		{
			XmlSerializer ser = new XmlSerializer(objectType, "");

			XmlReaderSettings settings = new XmlReaderSettings();
			XmlSerializerNamespaces blank = new XmlSerializerNamespaces();
			blank.Add("", "");
			using (StringReader sr = new StringReader(serializedata))
			using (XmlReader reader = XmlReader.Create(sr, settings))
			{
				return ser.Deserialize(reader);
			}
		}

		private static Dictionary<String, Type[]> MappedTypes;
		private static Type[] registeredTypes;

		static XmlHelper()
		{
			MappedTypes = new Dictionary<string, Type[]>();
			 registeredTypes = Assembly.GetExecutingAssembly().GetTypes()
			.Where(t => !t.IsInterface &&
				(typeof(IRuleNode).IsAssignableFrom(t) || typeof(IExtratorNode).IsAssignableFrom(t))).ToArray();
			foreach (Type type in registeredTypes)
			{
				XmlRootAttribute attr = (XmlRootAttribute) Attribute.GetCustomAttribute(type, typeof (XmlRootAttribute));
				MappedTypes.Add(attr.ElementName, (new Type[] {type}).Union(registeredTypes).ToArray());
			}
		}

		internal static void AddType(params Type[] types)
		{
			registeredTypes = registeredTypes.Union(types).ToArray();
			foreach (KeyValuePair<string, Type[]> pair in MappedTypes.ToList())
			{
				MappedTypes[pair.Key] = pair.Value.Union(types).ToArray();
			}
			foreach (Type type in types)
			{
				XmlRootAttribute attr = (XmlRootAttribute)Attribute.GetCustomAttribute(type, typeof(XmlRootAttribute));
				MappedTypes.Add(attr.ElementName, (new Type[] {type}).Union(registeredTypes).ToArray());
			}
		}

		/// <summary>
		/// Rule node are managed with simple deserialization xml
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static IRuleNode GetRuleNode(XElement element)
		{
			String elementName = element.Name.LocalName;
			return (IRuleNode)FromXml(element.ToString(), MappedTypes[elementName]);
		}

		/// <summary>
		/// Extractor nodes are more complicated objects, they needs to be managed differently
		/// because they have a 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static IExtratorNode GetExtractorNode(XElement element)
		{
			String elementName = element.Name.LocalName;
			IExtratorNode node = (IExtratorNode)FromXml(element.ToString(), MappedTypes[elementName]);
			return node;
		}
	}
}
