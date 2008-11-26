using MbUnit.Core;

namespace NHibernate.Linq.Tests
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			using (var auto = new AutoRunner())
			{
				auto.Run();
				auto.ReportToHtml();
			}
		}
	}
}