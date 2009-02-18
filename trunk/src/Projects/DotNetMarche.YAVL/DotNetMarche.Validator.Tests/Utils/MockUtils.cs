using DotNetMarche.Validator.Interfaces;
using Rhino.Mocks;

namespace DotNetMarche.Validator.Tests.Utils
{
	internal class MockUtils
	{
		internal static IValueExtractor CreateExtractor(MockRepository m, object valueToReturn)
		{
			var mock = m.CreateMock<IValueExtractor>();
			Expect.Call(mock.ExtractValue(null))
				.IgnoreArguments()
				.Return(valueToReturn);
			m.ReplayAll();
			return mock;
		}

		internal static IValueExtractor CreateExtractor(MockRepository m, object valueToReturn, object parameterValue)
		{
			var mock = m.CreateMock<IValueExtractor>();
			Expect.Call(mock.ExtractValue(parameterValue))
				.Return(valueToReturn);
			m.ReplayAll();
			return mock;
		}
	}
}