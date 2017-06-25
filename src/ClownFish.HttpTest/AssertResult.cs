using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpTest
{
	public sealed class AssertResult : AssertItem
	{
		public bool IsPassed { get; set; }

		public string ResultValue { get; set; }

		public string Message { get; set; }

		private AssertResult() { }

		public static AssertResult Create(AssertItem item)
		{
			AssertResult result = new AssertResult();
			result.Name = item.Name;
			result.AssertMode = item.AssertMode;
			result.Value = item.Value;
			return result;
		}
	}
}
