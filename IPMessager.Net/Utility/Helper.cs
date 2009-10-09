using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IPMessagerNet.Utility
{
	static class Helper
	{

		static Regex jsConventor = new Regex(@"['""\\]");

		/// <summary>
		/// 将字符串转换为JS格式
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ConvertJsString(string str)
		{
			if (string.IsNullOrEmpty(str)) return string.Empty;
			return jsConventor.Replace(str, (s) => { return string.Concat("\\", s.Value); }).Replace("\r", "\\r").Replace("\n", "\\n");
		}

		/// <summary>
		/// 将大小转换为文字表达形式
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static string ToSizeDescription(this ulong size)
		{
			double d = 1024 * 0.9;
			if (size < d) return String.Format("{0} 字节", size);
			else if (size < 0x400 * d) return String.Format("{0:#0.00} KB", size / 1024.0);
			else if (size < 0x100000 * d) return String.Format("{0:#0.00} MB", size / 1048576.0);
			else return string.Format("{0:#0.00} GB", size / (0x40000000 * 1.0));
		}
	}
}
