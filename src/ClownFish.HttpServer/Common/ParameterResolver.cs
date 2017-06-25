using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.Base.Reflection;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Attributes;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Common
{
	/// <summary>
	/// 用于执行Action时，构造方法参数的解析器
	/// </summary>
	public class ParameterResolver
	{
		private LazyObject<StringConverter> _stringConverter = new LazyObject<StringConverter>();

		/// <summary>
		/// 从HttpRequest中构造将要调用的方法的所有参数值
		/// </summary>
		/// <param name="method"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		public virtual object[] GetParameters(MethodInfo method, HttpRequest request)
		{
			var ps = method.GetParameters();
			if( ps.Length == 0 )
				return null;

			object[] parameters = new object[ps.Length];

			for(int i=0;i<ps.Length; i++ ) {
				ParameterInfo p = ps[i];
				parameters[i] = GetParameter(p, request);
			}

			return parameters;
		}


		/// <summary>
		/// 从HttpRequest中构造一个参数值
		/// </summary>
		/// <param name="p"></param>
		/// <param name="requst"></param>
		/// <returns></returns>
		public virtual object GetParameter(ParameterInfo p, HttpRequest requst)
		{
			// 参数的第一种取值方式：通过[FromBody]指定，反序列化请求流
			FromBodyAttribute attr = p.GetMyAttribute<FromBodyAttribute>();
			if( attr != null ) {
				return FromBodyDeserializeObject(p, requst);
			}


			// 参数的第二种取值方式：通过[FromRequest]指定，从HttpRequest中获取某个属性值
			FromRequestAttribute a2 = p.GetMyAttribute<FromRequestAttribute>();
			if( a2 != null ) {
				return FromRequestPropertyGetValue(p, requst);
			}


			// 参数的第三种取值方式：根据参数名，从各种数据集合中获取（Route, QueryString, Form, Headers）
			string value = requst[p.Name];
			if( value != null ) {
				// 非序列化的参数，允许可空类型
				return _stringConverter.Instance.ToObject(value, p.ParameterType.GetRealType());
			}

			// 查找失败
			return null;
		}

		/// <summary>
		/// 从HttpRequest对象中获取某个属性，属性的名称就是参数的名称
		/// </summary>
		/// <param name="p"></param>
		/// <param name="requst"></param>
		/// <returns></returns>
		protected virtual object FromRequestPropertyGetValue(ParameterInfo p, HttpRequest requst)
		{
            PropertyInfo pi = requst.GetType().GetProperty(p.Name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if( pi == null )
                throw new ArgumentException("参数名对应的属性不存在：" + p.Name);

            return pi.FastGetValue(requst);
            //return requst.GetPropertyValue(p.Name);
		}

		/// <summary>
		/// 从请求流中反序列化构造参数值
		/// </summary>
		/// <param name="p"></param>
		/// <param name="requst"></param>
		/// <returns></returns>
		public virtual object FromBodyDeserializeObject(ParameterInfo p, HttpRequest requst)
		{
			// 从请求流中反序列化对象中，要注意三点：
			// 1、忽略参数的名称
			// 2、直接使用参数类型，不做可空类型处理
			// 3、仅支持 JSON, XML 的数据格式

			RequestDataType dataType = requst.GetDataType();
			if( dataType == RequestDataType.Json ) {
				string text = requst.GetPostText();
				return JsonExtensions.FromJson(text, p.ParameterType);
			}

			if( dataType == RequestDataType.Xml ) {
				string text = requst.GetPostText();
				return XmlExtensions.FromXml(text, p.ParameterType);
			}

			// 仅仅是需要读取整个请求流字符串，
			// 而且目标类型已经是字符串，就没有必要反序列化了，所以就直接以字符串返回
			if( p.ParameterType == typeof(string) ) {
				return requst.GetPostText();
			}

			throw new NotSupportedException("[FromBody]标记只能配合 JSON/XML 数据格式来使用。");
		}
	}
}
