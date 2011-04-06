using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetMarche.Validator.Validators;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Type;

namespace DotNetMarche.Validator.Extras
{
	public class ValidatorFromMetadata
	{

		public static Core.Validator GetValidatorFromSession(ISessionFactory sessionFactory)
		{
			var allDefindedClasses = sessionFactory.GetAllClassMetadata();
			Core.Validator validator = new Core.Validator();
			foreach (KeyValuePair<string, IClassMetadata> pair in allDefindedClasses)
			{
				IClassMetadata metadata = pair.Value;
				foreach (string propertyName in metadata.PropertyNames)
				{
					IType propertyType = metadata.GetPropertyType(propertyName);
					StringType st = propertyType as StringType;
					if (st != null)
					{
						if (st.SqlType.Length > 0)
						{
							validator.AddRule(Rule.For(metadata.GetMappedClass(EntityMode.Poco))
								.OnMember(propertyName)
										.MaxLength(st.SqlType.Length)
										.Message(String.Format(
										"Property {0} have a maximum length of {1}",
											propertyName,
													st.SqlType.Length)));
						}
					}
				}
			}
			return validator;
		}
	}
}
