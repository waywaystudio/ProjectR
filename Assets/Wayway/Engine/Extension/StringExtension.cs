using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wayway.Engine
{
    public static class StringExtension
    {
        /// <summary>
        /// Changing String to CamelCase
        /// </summary>
        public static string ToCamelCase(this string original)
        {
            if (string.IsNullOrEmpty(original) || char.IsLower(original, 0))
            {
                return original;
            }

            return char.ToLowerInvariant(ToPascalCase(original)[0]) + original[1..];
        }

        /// <summary>
        /// Changing String to PascalCase
        /// </summary>
        public static string ToPascalCase(this string original)
        {
            var invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            var whiteSpace = new Regex(@"(?<=\s)");
            var startsWithLowerCaseChar = new Regex("^[a-z]");
            var firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            var upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with underscore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                .Split(new [] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));
            
            var result = string.Concat(pascalCase);

            return result.Length <= 2
                ? result.ToUpper()
                : result;
        }
    }
}
