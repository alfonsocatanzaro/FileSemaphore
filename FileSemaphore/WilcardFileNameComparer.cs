using System.Text.RegularExpressions;

namespace System.Threading {
    public class WilcardFileNameComparer {
        private readonly Regex regEx;
        public WilcardFileNameComparer (string pattern) {
            regEx = new Regex (pattern
                .Replace (".", "\\.")
                .Replace ("?", ".")
                .Replace ("*", ".*"),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        public bool IsMatch (string filename) {
            return regEx.IsMatch (filename);
        }
    }
}