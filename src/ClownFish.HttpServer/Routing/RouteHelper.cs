using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.Base.Reflection;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Routing
{
	internal static class RouteHelper
	{
		private static readonly Regex s_regex = new Regex(@"{(\w+)}", RegexOptions.Compiled);

		/// <summary>
		/// 将包含了占位符模式的字符串翻译成等效的正则表达式
		/// </summary>
		/// <param name="pattern"></param>
		/// <returns></returns>
		public static Regex CreateRegex(string pattern)
		{
			string newString = s_regex.Replace(pattern, @"(?<$1>\w+)");
			return new Regex(newString, RegexOptions.Compiled | RegexOptions.IgnoreCase);


			// input:   /page/{id}/{year}-{month}-{day}.aspx
			// output:  /page/(?<id>\w+)/(?<year>\w+)-(?<month>\w+)-(?<day>\w+).aspx
		}



		public static bool IsMatchRequest(HttpContext context, Regex regex)
		{
			if( regex == null )
				throw new InvalidOperationException(/*  不可能发生的事情，除非是代码有BUG  */);


			Match m = regex.Match(context.Request.Path);
			if( m.Success ) {
				context.Request.RegexMatch = m;		// 保存匹配的结果
				return true;
			}
			return false;
		}


		public static bool IsMatchRequest(HttpContext context, RoutingService route)
		{
			if( route.Attr.Regex == null )
				throw new InvalidOperationException(/*  不可能发生的事情，除非是代码有BUG  */);

			// 先检查类型中标记的[Route]是否匹配
			Match match = route.Attr.Regex.Match(context.Request.Path);
			if( match.Success == false )
				return false;

			// 获取 action 名字
			string action = match.Groups["action"].Value;
			if( action == null )
				// URL模式中没有指定 {action}占位符，就用HttpMethod来代替
				action = context.Request.HttpMethod;


			// 在类型中查找匹配的 action
			MethodInfo method = null;
			var methods = route.ServiceType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			foreach(MethodInfo m in methods ) {

				// RouteIgnoreAttribute 优先级最高。
				if( m.GetMyAttribute<RouteIgnoreAttribute>() != null )
					continue;

				// 别名比较
				RouteActionAttribute a2 = m.GetMyAttribute<RouteActionAttribute>();
				if( a2 != null && string.IsNullOrEmpty(a2.Name)== false ) {
					if( action.EqualsIgnoreCase(a2.Name) ) {
						method = m;
						break;
					}
				}

				// 直接用方法名比较
				if( action.EqualsIgnoreCase(m.Name) ) {
					method = m;
					break;
				}
			}

			// 匹配成功
			if( method != null ) {
				RoutingService obj = route.Clone(method);
				obj.SetHandler(context);

				context.Request.RegexMatch = match;     // 保存匹配的结果
				return true;
			}


			// 类型上标记的[Route("....")]匹配成功，但是没有找到合适的方法，所以直接退出整个搜索过程
			throw new RouteMatchExistException();
		}

	}
}
