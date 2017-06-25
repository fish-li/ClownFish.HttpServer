using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClownFish.HttpTest
{
	public abstract class StringComparer
	{
		public abstract bool IsRight(string a, string b);
	}

	public static class StringComparerFactory
	{
		public static StringComparer Create(string assertMode)
		{
			switch( assertMode ) {
				case "=":
					return new StringComparerEqualsIgnoreCase();

				case "==":
					return new StringComparerEquals();

				case "include":
					return new StringComparerInclude();

				case "includeIgnoreCase":
					return new StringComparerIncludeIgnoreCase();

				case "regex":
					return new StringComparerRegex();

				case "regexIgnoreCase":
					return new StringComparerRegexIgnoreCase();

				default:
					throw new NotSupportedException("不支持的比较模式：" + assertMode);

			}
		}
	}

	public sealed class StringComparerEquals : StringComparer
	{
		public override bool IsRight(string a, string b)
		{
			return string.Equals(a, b);
		}
	}
	public sealed class StringComparerEqualsIgnoreCase : StringComparer
	{
		public override bool IsRight(string a, string b)
		{
			return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}
	}


	public sealed class StringComparerInclude : StringComparer
	{
		public override bool IsRight(string a, string b)
		{
			if( a == null || b == null )
				return false;

			return a.IndexOf(b) >= 0;
		}
	}
	public sealed class StringComparerIncludeIgnoreCase : StringComparer
	{
		public override bool IsRight(string a, string b)
		{
			if( a == null || b == null )
				return false;

			return a.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0;
		}
	}


	public sealed class StringComparerRegex : StringComparer
	{
		public override bool IsRight(string a, string b)
		{
			if( a == null || b == null )
				return false;

			return Regex.IsMatch(a, b);
		}
	}

	public sealed class StringComparerRegexIgnoreCase : StringComparer
	{
		public override bool IsRight(string a, string b)
		{
			if( a == null || b == null )
				return false;

			return Regex.IsMatch(a, b, RegexOptions.IgnoreCase);
		}
	}
}
