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
	[XmlInclude(typeof (Order))]
	[SoapInclude(typeof (Order))]
	public class AbstractShipper
	{
		public virtual int ShipperID { get; set; }

		public virtual string CompanyName { get; set; }

		public virtual string Phone { get; set; }

		public virtual IList Orders { get; set; }
	}

	[Serializable]
	public class Shipper : AbstractShipper
	{
	}
}