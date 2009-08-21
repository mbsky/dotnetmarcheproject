using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using DotNetMarche.Validator.Configuration.Xml;
using DotNetMarche.Validator.Configuration.Xml.Extractors;
using DotNetMarche.Validator.Configuration.Xml.Rules;
using NUnit.Framework;

namespace DotNetMarche.Validator.Tests.ConfigurationTests
{
	[TestFixture]
	public class XmlRuleNodeTests
	{ 
		[Test]
		public void DeserializationRequired()
		{
			XElement element = XElement.Parse("<required />");
			IRuleNode rule = XmlHelper.GetRuleNode(element);
			Assert.That(rule, Is.TypeOf<RequiredRuleNode>());
		}

		[Test]
		public void RangeNodeDesandSer()
		{
			RangeRuleNode node = new RangeRuleNode() { Max = 100, Min = -100 };
			String ser = XmlHelper.ToXml(node);
			RangeRuleNode deser = XmlHelper.FromXml<RangeRuleNode>(ser);
			Assert.That(deser.Min, Is.EqualTo(-100));
			Assert.That(deser.Max, Is.EqualTo(100));
		}

		[Test]
		public void RangeNodeDesandSer2()
		{
			RangeRuleNode node = new RangeRuleNode() { Max = 100, Min = -100 };
			String ser = XmlHelper.ToXml(node);
			RangeRuleNode deser = (RangeRuleNode)XmlHelper.FromXml(ser, typeof(RangeRuleNode));
			Assert.That(deser.Min, Is.EqualTo(-100));
			Assert.That(deser.Max, Is.EqualTo(100));
		}

		[Test]
		public void RangeNodeDesandSer3()
		{
			RangeRuleNode node = new RangeRuleNode() { Max = 100, Min = -100 };
			String ser = XmlHelper.ToXml(node);
			RangeRuleNode deser = (RangeRuleNode)XmlHelper.FromXml(ser, new Type[] { typeof(RangeRuleNode) });
			Assert.That(deser.Min, Is.EqualTo(-100));
			Assert.That(deser.Max, Is.EqualTo(100));
		}

		[Test]
		public void RangeNodeDesandSer4()
		{
			RangeRuleNode node = new RangeRuleNode() { Max = 100, Min = -100 };
			String ser = XmlHelper.ToXml(node);
			XElement element = XElement.Parse(ser);
			RangeRuleNode deser = (RangeRuleNode)XmlHelper.FromXml(element.ToString(), new Type[] { typeof(RangeRuleNode) });
			Assert.That(deser.Min, Is.EqualTo(-100));
			Assert.That(deser.Max, Is.EqualTo(100));
		}


		[Test]
		public void RangeNode()
		{
			RangeRuleNode node = new RangeRuleNode() { Max = 100, Min = -100 };
			String ser = XmlHelper.ToXml(node);
			XElement element = XElement.Parse(ser);
			XmlHelper.FromXml(element.ToString(), new Type[] { typeof(RangeRuleNode), typeof(RequiredRuleNode) });
			IRuleNode rule = XmlHelper.GetRuleNode(element);
			Assert.That(rule, Is.TypeOf<RangeRuleNode>());
			node = (RangeRuleNode)rule;
			Assert.That(node.Min, Is.EqualTo(-100));
			Assert.That(node.Max, Is.EqualTo(100));
		}

		[Test]
		public void ExtractorSerAndDeser()
		{
			MemberExtractorNode sut = new MemberExtractorNode();
			String ser = XmlHelper.ToXml(sut);
			XElement element = XElement.Parse(ser);
			MemberExtractorNode deser = (MemberExtractorNode)XmlHelper.GetExtractorNode(element);
			Assert.That(deser.RuleNodes, Has.Count.EqualTo(0));

		}

		[Test]
		public void DeserializationOfExtractorRequiredRule()
		{
			XElement element = XElement.Parse("<member name='field'><required /></member>");
			IExtratorNode rule = XmlHelper.GetExtractorNode(element);
			Assert.That(rule, Is.TypeOf<MemberExtractorNode>());
			MemberExtractorNode extractor = (MemberExtractorNode)rule;
			Assert.That(extractor.RuleNodes, Has.Count.EqualTo(1));
			Assert.That(extractor.RuleNodes[0], Is.InstanceOf<RequiredRuleNode>());
		}

		[Test]
		public void DeserializationOfExtractorDoubledRule()
		{
			XElement element = XElement.Parse("<member name='field'><required /><range><min>100</min><max>200</max></range></member>");
			IExtratorNode rule = XmlHelper.GetExtractorNode(element);
			Assert.That(rule, Is.TypeOf<MemberExtractorNode>());
			MemberExtractorNode extractor = (MemberExtractorNode)rule;
			Assert.That(extractor.RuleNodes, Has.Count.EqualTo(2));
			Assert.That(extractor.RuleNodes[0], Is.InstanceOf<RequiredRuleNode>());
			Assert.That(extractor.RuleNodes[1], Is.InstanceOf<RangeRuleNode>());
		}

		[Test]
		public void DeserializationOfExtractorWeird()
		{
			XmlHelper.AddType(typeof(WeirdRuleNode));
			XElement element = XElement.Parse("<member name='field'><required /><weird /></member>");
			IExtratorNode rule = XmlHelper.GetExtractorNode(element);
			Assert.That(rule, Is.TypeOf<MemberExtractorNode>());
			MemberExtractorNode extractor = (MemberExtractorNode)rule;
			Assert.That(extractor.RuleNodes, Has.Count.EqualTo(2));
			Assert.That(extractor.RuleNodes[0], Is.InstanceOf<RequiredRuleNode>());
			Assert.That(extractor.RuleNodes[1], Is.InstanceOf<WeirdRuleNode>());
		}

		[Test]
		public void DeserializationOfExtractorEmpty()
		{
			XElement element = XElement.Parse("<member name='field'></member>");
			IExtratorNode rule = XmlHelper.GetExtractorNode(element);
			Assert.That(rule, Is.TypeOf<MemberExtractorNode>());
			MemberExtractorNode extractor = (MemberExtractorNode) rule;
			Assert.That(extractor.RuleNodes, Has.Count.EqualTo(0));
		}

		#region Innerclasses

		[Serializable, XmlRoot("weird", Namespace = "")]
		public class WeirdRuleNode : IRuleNode
		{
			#region RuleNode Members

			public Validators.Rule Configure(Validators.Rule rule)
			{
				return rule.SetRequired();
			}

			#endregion
		}
		#endregion
	}
}
