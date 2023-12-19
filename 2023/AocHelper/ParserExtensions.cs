using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AocHelper
{
    public static class ParserExtensions
    {
        public static T[,] GetMap<T>(this string[] lines)
        {
            var map = new T[lines[0].Length, lines.Length];

            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Length; y++)
                {
                    map[x, y] = (T)Convert.ChangeType(lines[y][x].ToString(), typeof(T));
                }
            }

            return map;
        }

        public static T[,] GetMap<T>(this string[] lines, out int xLength, out int yLength)
        {
            yLength = lines.Length;
            xLength = lines[0].Length;

            return GetMap<T>(lines);
        }

        public static string Parse<T>(this string input, string prefix, out T value, string suffix = "") where T : struct
        {
            return ParseInternal<T>(input, prefix, out value, suffix);
        }

        public static string Parse(this string input, string prefix, out string value, string suffix = "")
        {
            var result = ParseInternal<string>(input, prefix, out string? foundValue, suffix);
            value = foundValue ?? string.Empty;
            return result;
        }

        public static string ParseMany<T>(this string input, string prefix, string delimeter, out T[] values, string suffix = "") where T : struct
        {
            var result = ParseMany(input, prefix, delimeter, out string[] foundValues, suffix);
            values = new T[foundValues.Length];
            for (int i = 0; i < foundValues.Length; i++)
            {
                values[i] = (T)Convert.ChangeType(foundValues[i], typeof(T));
            }

            return result;
        }

        public static string ParseMany(this string input, string prefix, string delimeter, out string[] values, string suffix = "")
        {
            values = new string[0];
            var result = ParseInternal<string>(input, prefix, out string? foundValue, suffix);
            if (string.IsNullOrEmpty(foundValue))
            {
                return result;
            }

            values = foundValue.Split(delimeter);

            return result;
        }

        private static string ParseInternal<T>(this string input, string prefix, out T? value, string suffix = "")
        {
            value = default;
            suffix ??= "";
            prefix ??= "";

            if (input == string.Empty)
            {
                return string.Empty;
            }

            int startIndex = 0;
            if (!string.IsNullOrEmpty(prefix))
            {
                startIndex = input.IndexOf(prefix);
                if (startIndex == -1)
                {
                    throw new MatchNotFoundException(prefix, input);
                }

                startIndex += prefix.Length;
            }

            input = input.Substring(startIndex);

            int endIndex = input.Length;
            if (!string.IsNullOrEmpty(suffix))
            {
                endIndex = input.IndexOf(suffix);
                if (endIndex == -1)
                {
                    throw new MatchNotFoundException(suffix, input);
                }
            }

            string target = input.Substring(0, endIndex);
            value = (T)Convert.ChangeType(target, typeof(T));

            int nextStartIndex = endIndex + suffix.Length;
            return input.Substring(nextStartIndex);
        }

        public class MatchNotFoundException : Exception
        {
            public MatchNotFoundException(string lookingFor, string input)
                : base($"Could not find string \"{lookingFor}\" in string \"{input}\"")
            {

            }
        }
    }
}
