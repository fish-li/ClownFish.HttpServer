using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpTest
{
	internal sealed class BodyAssertChecker
	{
		private StringComparer _comparer;

		private BodyAssertChecker() { }

		public static BodyAssertChecker Create(string assertMode)
		{
			BodyAssertChecker check = new BodyAssertChecker();
			check._comparer = StringComparerFactory.Create(assertMode);
			return check;
		}

		public AssertResult Execute(ResponseBodyAssert test, ResponseResult response)
		{
			AssertResult result = AssertResult.Create(test);

			if( response.ResponseText == null ) {
				result.IsPassed = false;
				result.Message = "没有获取到响应内容。";
				return result;
			}

			string value = null;
			if( test.Name.Equals("Text", StringComparison.OrdinalIgnoreCase) )
				value = response.ResponseText;

			else if( test.Name.Equals("Length", StringComparison.OrdinalIgnoreCase) )
				value = response.ResponseText.Length.ToString();

			else {
				result.IsPassed = false;
				result.Message = "不支持的属性名称：" + test.Name;
				return result;
			}
						

			if( _comparer.IsRight(value, test.Value) == false ) {
				result.IsPassed = false;
				result.Message = "断言失败。";
				return result;
			}


			result.IsPassed = true;
			return result;
		}
	}
}
