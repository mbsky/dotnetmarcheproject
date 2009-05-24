using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetMarche.TestHelpers
{
	public static class Generator
	{
		private static Random random = new Random((Int32) DateTime.Now.Ticks%Int32.MaxValue);

		private static Int32 seed = 0;
		private static Int32 Seed
		{
			get { return ++seed; }
		}
		public static String RandomString(Int32 stringLength)
		{
			StringBuilder sb = new StringBuilder(stringLength);
			for(Int32 I = 0; I < stringLength; ++I)
			{
				Int32 rnd = random.Next();
				if (rnd % 2 == 0)
				{
					//Generate Upper Case Char
					sb.Append(Convert.ToChar(rnd%21 + 'A'));
				} else
				{
					//Generate Lower Case Char
					sb.Append(Convert.ToChar(rnd % 21 + 'a'));
				}
			}
			return sb.ToString();
		}


		/// <summary>
		/// Even if strings are random, this is needed to make the test 
		/// run good
		/// </summary>
		/// <param name="stringLength"></param>
		/// <returns></returns>
		public static String RandomStringUnique(Int32 stringLength)
		{
			String seed = Seed.ToString();
			return RandomString(stringLength - seed.Length) + seed;
		}
	}
}
