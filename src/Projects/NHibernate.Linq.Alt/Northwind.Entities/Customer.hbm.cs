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
	[XmlInclude(typeof (CustomerCustomerDemo))]
	[SoapInclude(typeof (CustomerCustomerDemo))]
	[XmlInclude(typeof (Order))]
	[SoapInclude(typeof (Order))]
	public class AbstractCustomer
	{
		public virtual string CustomerID { get; set; }

		public virtual string Address { get; set; }

		public virtual string City { get; set; }

		public virtual string CompanyName { get; set; }

		public virtual string ContactName { get; set; }

		public virtual string ContactTitle { get; set; }

		public virtual string Country { get; set; }

		public virtual string Fax { get; set; }

		public virtual string Phone { get; set; }

		public virtual string PostalCode { get; set; }

		public virtual string Region { get; set; }

		public virtual IList CustomerCustomerDemos { get; set; }

		public virtual IList Orders { get; set; }
	}

	[Serializable]
	public class Customer : AbstractCustomer
	{
	}
}