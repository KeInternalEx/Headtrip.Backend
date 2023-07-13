using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Headtrip.SQLUtilities
{

    public static class StringHelpers
    {
        private static Dictionary<string, string> escapeMapping = new Dictionary<string, string>()
    {
        {"\"", @"\\\"""},
        {"\\\\", @"\\"},
        {"\a", @"\a"},
        {"\b", @"\b"},
        {"\f", @"\f"},
        {"\n", @"\n"},
        {"\r", @"\r"},
        {"\t", @"\t"},
        {"\v", @"\v"},
        {"\0", @"\0"},
    };

        private static Regex escapeRegex = new Regex(string.Join("|", escapeMapping.Keys.ToArray()));

        public static string Escape(this string s)
        {
            return escapeRegex.Replace(s, EscapeMatchEval);
        }

        private static string EscapeMatchEval(Match m)
        {
            if (escapeMapping.ContainsKey(m.Value))
            {
                return escapeMapping[m.Value];
            }
            return escapeMapping[Regex.Escape(m.Value)];
        }
    }

}
