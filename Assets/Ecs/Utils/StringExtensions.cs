using System;
using System.IO;

namespace Ecs.Utils
{
    public static class StringExtension {
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        public static T ToEnum<T>(this string s)
            => s.IsNullOrEmpty() || s.Length < 2 ? default(T) : (T) Enum.Parse(typeof(T), s);

        public static bool ExelToBool(this string s) => s == "YES";

        public static string AddPath(this string s, object path) => Path.Combine(s, path.ToString());
    }
}