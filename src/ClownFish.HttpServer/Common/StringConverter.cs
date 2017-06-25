using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.Reflection;

namespace ClownFish.HttpServer.Common
{
	/// <summary>
	/// 将字符串转换成其它类型的转换器
	/// </summary>
	public class StringConverter
	{
		private static readonly char[] s_stringSlitArray = new char[] { ',' };

		/// <summary>
		/// 将字符串转换成指定的数据类型
		/// </summary>
		/// <param name="value"></param>
		/// <param name="conversionType"></param>
		/// <returns></returns>
		public virtual object ToObject(string value, Type conversionType)
		{
			// 注意：这个方法应该与ReflectionExtensions2.IsSupportableType保持一致性。
			// 如果 conversionType.IsSupportableType() 返回 true，这里应该能转换，除非字符串的格式不正确。

			if( conversionType == typeof(string) )
				return value;

			if( value == null || value.Length == 0 ) {
				// 空字符串根本不能做任何转换，所以直接返回null
				return null;
			}

			if( conversionType == typeof(string[]) )
				// 保持与NameValueCollection的行为一致。
				return value.Split(s_stringSlitArray, StringSplitOptions.RemoveEmptyEntries);


			if( conversionType == typeof(Guid) )
				return new Guid(value);

			if( conversionType.IsEnum )
				return Enum.Parse(conversionType, value);

			// 尝试使用类型的隐式转换（从字符串转换）
			if( IsSupportableType(conversionType) == false ) {
				MethodInfo stringImplicit = GetStringImplicit(conversionType);
				if( stringImplicit != null )
					return stringImplicit.FastInvoke(null, value);
			}

			if( conversionType == typeof(byte[]) )
				return Convert.FromBase64String(value);


			// 如果需要转换其它的数据类型，请重写下面的方法。
			return DefaultChangeType(value, conversionType);
		}


		/// <summary>
		/// 判断是否是一个可支持的参数类型。仅包括：基元类型，string ，decimal，DateTime，Guid, string[], 枚举
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual bool IsSupportableType(Type type)
		{
			return type.IsPrimitive
				|| type == typeof(string)
				|| type == typeof(DateTime)
				|| type == typeof(decimal)
				|| type == typeof(Guid)
				|| type.IsEnum
				|| type == typeof(string[])
				|| type == typeof(byte[])
				;
		}


		/// <summary>
		/// 调用.NET的默认实现，将字符串转换成指定的数据类型。
		/// </summary>
		/// <param name="value"></param>
		/// <param name="conversionType"></param>
		/// <returns></returns>
		protected virtual object DefaultChangeType(string value, Type conversionType)
		{
			// 为了简单，直接调用 .net framework中的方法。
			// 如果转换失败，将会抛出异常。

			// 如果需要转换其它的数据类型，请重写这个方法。

			return Convert.ChangeType(value, conversionType);
		}


		/// <summary>
		/// 判断指定的类型是否能从String类型做隐式类型转换，如果可以，则返回相应的方法
		/// </summary>
		/// <param name="conversionType"></param>
		/// <returns></returns>
		private MethodInfo GetStringImplicit(Type conversionType)
		{
			MethodInfo m = conversionType.GetMethod("op_Implicit",
													BindingFlags.Static | BindingFlags.Public, null,
													new Type[] { typeof(string) }, null);

			if( m != null && m.IsSpecialName && m.ReturnType == conversionType )
				return m;
			else
				return null;
		}

	}
}
