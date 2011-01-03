using System;
using DotNetMarche.Validator.Interfaces;
using Rhino.Mocks;

namespace DotNetMarche.Validator.Tests.Utils
{
	internal class MockUtils
	{
		internal static IValueExtractor CreateExtractor(MockRepository m, object valueToReturn)
		{
			//var mock = m.CreateMock<IValueExtractor>();
			//Expect.Call(mock.ExtractValue(null))
			//   .IgnoreArguments()
			//   .Return(valueToReturn);
			//m.ReplayAll();
			//return mock;
			var mock = MockRepository.GenerateStub<IValueExtractor>();
			mock.Expect(ve => mock.ExtractValue(null))
				.IgnoreArguments()
				.Return(valueToReturn);
			mock.Expect(o => o.SourceName).Return("property");
			return mock;
		}

		internal static IValueExtractor CreateExtractor(MockRepository m, object valueToReturn, object parameterValue)
		{
			var mock = MockRepository.GenerateStub<IValueExtractor>();
			mock.Expect(ve => mock.ExtractValue(parameterValue))
				.Return(valueToReturn);
			mock.Expect(o => o.SourceName).Return("property");
			m.ReplayAll();
			return mock;
		}

		public static IValueExtractor CreateExtractorForBasicRange(MockRepository m, object valueToReturn)
		{
			var mock = MockRepository.GenerateStub<IValueExtractor>();
			mock.Expect(ve => mock.ExtractValue(null))
				.IgnoreArguments()
				.Return(valueToReturn);
			mock.Expect(ve => ve.SourceName).Return("Property");

			return mock;
		}
	}
}