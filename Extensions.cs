using System.Collections.Generic;
using System.Text;

namespace SentenceStitcher
{
    public static class Extensions
    {
        public static IEnumerable<string> SplitAndKeep(this string s, char[] delims)
        {
            int start = 0, index;

            while ((index = s.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                    yield return s.Substring(start, index - start);
                yield return s.Substring(index, 1);
                start = index + 1;
            }

            if (start < s.Length)
            {
                yield return s.Substring(start);
            }
        }

        public static T GetLast<T>(this List<T> l)
        {
            return l[l.Count - 1];
        }

        public static string ToFormattedString<T>(this List<T> s, char surround = new char())
        {
            var builder = new StringBuilder();

            builder.Append('[');

            for (var i = 0; i < s.Count; i++)
            {
                if (i < s.Count - 1)
                    builder.AppendFormat("{1}{0}{1}, ", s[i], surround);
                else
                    builder.AppendFormat("{1}{0}{1}", s[i], surround);
            }

            builder.Append(']');

            return builder.ToString();
        }
    }
}
