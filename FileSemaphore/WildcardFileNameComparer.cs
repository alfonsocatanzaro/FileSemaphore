using System.Text.RegularExpressions;

namespace System.Threading {
    /// <summary>
    /// Provide a wildcard pattern matching for file names
    /// </summary>
    public class WildcardFileNameComparer {
        private readonly Regex regEx;

        /// <summary>
        /// Create an instance of <see cref="WildcardFileNameComparer"/>
        /// </summary>
        /// <param name="pattern">File pattern with wildcard</param>
        public WildcardFileNameComparer (string pattern) {
            regEx = new Regex (pattern
                .Replace (".", "\\.")
                .Replace ("?", ".")
                .Replace ("*", ".*"),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// Check if file name match with the pattern
        /// </summary>
        /// <param name="filename">File name to check</param>
        /// <returns>True if file name match with the pattern</returns>
        public bool IsMatch (string filename) {
            return regEx.IsMatch (filename);
        }
    }
}