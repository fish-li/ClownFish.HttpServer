using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer.Common;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Result
{
	/// <summary>
	/// 用于将一个非IActionResult类型对象转换成IActionResult类型的转换器实现，
	/// 可以继承此类型来实现个性化的定制转换过程。
	/// </summary>
	public class ResultConvert
	{
		/// <summary>
		/// 将一个对象转换成IActionResult实例
		/// </summary>
		/// <param name="value"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual IActionResult Convert(object value, HttpContext context)
		{
			if( value == null )
				throw new ArgumentNullException(nameof(value));

			Type t = value.GetType();

			if( t == typeof(string) )
				return ConvertString(value);

			if( t == typeof(DateTime) )
				return ConvertDateTime(value);

			if( t.IsPrimitive || t.IsCommonValueType() )
				return ConvertValue(value);

			return ConvertObject(value, context);
		}

		/// <summary>
		/// 将一个字符串转换成IActionResult实例（默认采用TextResult类型）
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual IActionResult ConvertString(object value)
		{
			return new TextResult(value);
		}

		/// <summary>
		/// 将一个DateTime转换成IActionResult实例（默认采用TextResult类型）
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual IActionResult ConvertDateTime(object value)
		{
			DateTime time = (DateTime)value;
			return new TextResult(time.ToTimeString());
		}


		/// <summary>
		/// 将.NET内置的值类型对象转换成IActionResult实例（默认采用TextResult类型），覆盖类型：
		/// 基元类型： Boolean、Byte、SByte、Int16、UInt16、Int32、UInt32、Int64、UInt64、IntPtr、UIntPtr、Char、Double 和 Single。
		/// 以及结构体： Guid，decimal， Enum
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected virtual IActionResult ConvertValue(object value)
		{
			return new TextResult(value);
		}

		/// <summary>
		/// 将一个对象转换成IActionResult实例（默认采用JsonResult类型）
		/// </summary>
		/// <param name="value"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual IActionResult ConvertObject(object value, HttpContext context)
		{
			return new JsonResult(value);
		}
	}
}
