﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Xml.Serialization;

namespace Northwind.Entities
{
	[Serializable]
	[XmlInclude(typeof (Employee))]
	[SoapInclude(typeof (Employee))]
	[XmlInclude(typeof (EmployeeTerritory))]
	[SoapInclude(typeof (EmployeeTerritory))]
	[XmlInclude(typeof (Order))]
	[SoapInclude(typeof (Order))]
	public class AbstractEmployee
	{
		public virtual int EmployeeID { get; set; }

		public virtual string Address { get; set; }

		[SoapElement(IsNullable = true)]
		public virtual DateTime? BirthDate { get; set; }

		public virtual string City { get; set; }

		public virtual string Country { get; set; }

		public virtual string Extension { get; set; }

		public virtual string FirstName { get; set; }

		[SoapElement(IsNullable = true)]
		public virtual DateTime? HireDate { get; set; }

		public virtual string HomePhone { get; set; }

		public virtual string LastName { get; set; }

		public virtual string Notes { get; set; }

		public virtual byte[] Photo { get; set; }

		public virtual string PhotoPath { get; set; }

		public virtual string PostalCode { get; set; }

		public virtual string Region { get; set; }

		public virtual string Title { get; set; }

		public virtual string TitleOfCourtesy { get; set; }

		public virtual IList Employees { get; set; }

		public virtual IList EmployeeTerritories { get; set; }

		public virtual IList Orders { get; set; }
	}

	[Serializable]
	public class Employee : AbstractEmployee
	{
	}
}