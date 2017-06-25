using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Common
{
	internal static class DictionaryExtensions
	{
		public static void AddValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			try {
				dict.Add(key, value);
			}
			catch( ArgumentException ex ) {
				throw new ArgumentException(string.Format("往集合中插入元素时发生了异常，当前Key={0}", key), ex);
			}
		}



		public static void AddValue(this Hashtable table, object key, object value)
		{
			try {
				table.Add(key, value);
			}
			catch( ArgumentException ex ) {
				throw new ArgumentException(string.Format("往集合中插入元素时发生了异常，当前Key={0}", key), ex);
			}
		}

	}
}
