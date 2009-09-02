using System;

namespace DotNetMarche.TestHelpers.BaseTests
{
	public interface IBaseTestFixture
	{
		void DisposeAtTheEndOfTest(IDisposable disposableObject);
		void DisposeAtTheEndOfFixture(IDisposable disposableObject);
		void ExecuteAtTheEndOfTest(Action action);
		void ExecuteAtTheEndOfFixture(Action action);
		void SetIntoTestContext(String key, Object value);
		T GetFromTestContext<T>(String key);
	}
}