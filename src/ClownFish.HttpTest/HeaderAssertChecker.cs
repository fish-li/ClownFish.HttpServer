using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpTest
{
	internal sealed class HeaderAssertChecker
	{
		private StringComparer _comparer;

		private HeaderAssertChecker() { }

		public static HeaderAssertChecker Create(string assertMode)
		{
			HeaderAssertChecker check = new HeaderAssertChecker();
			check._comparer = StringComparerFactory.Create(assertMode);
			return check;
		}

		public AssertResult Execute(ResponseHeaderAssert test, ResponseResult response)
		{
			AssertResult result = AssertResult.Create(test);

			if( response.Headers == null || response.Headers.Count == 0 ) {
				result.IsPassed = false;
				result.Message = "没有任何响应头。";
				return result;
			}

			string value = response.Headers[test.Name];
			if( value == null ) {
				result.IsPassed = false;
				result.Message = "没有匹配的响应头：" + test.Name;
				return result;
			}
			result.ResultValue = value;

			if( _comparer.IsRight(value, test.Value) == false ) {
				result.IsPassed = false;
				result.Message = "断言失败：" + $"{test.Name} {test.AssertMode} {test.Value}";
				return result;
			}
			
			result.IsPassed = true;
			return result;
		}

	}

	

}
