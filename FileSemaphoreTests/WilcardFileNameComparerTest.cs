using System.Threading;
using Xunit;

namespace FileSemaphoreTests {
    public class WilcardFileNameComparerTest {
        [Theory]
        [InlineData ("*.*", "File.ext")]
        [InlineData ("name.*", "name.ext")]
        [InlineData ("NAME.*", "name.ext")]
        [InlineData ("name.*", "NAME.EXT")]
        [InlineData ("prefix*.ext", "prefix123.ext")]
        [InlineData ("*postfix.ext", "123postfix.ext")]
        [InlineData ("prefix*postfix.ext", "prefix123postfix.ext")]
        [InlineData ("test?test.???", "test_test.aaa")]
        public void ShouldMatch (string pattern, string filename) {
            WilcardFileNameComparer wc = new WilcardFileNameComparer (pattern);
            Assert.True (wc.IsMatch (filename));
        }

        [Theory]
        [InlineData ("name.*", "name")]
        [InlineData ("prefix*.ext", "presfix123.ext")]
        [InlineData ("*postfix.ext", "123postsfix.ext")]
        [InlineData ("test?test.???", "testtest.aaa")]
        public void ShouldntMatch (string pattern, string filename) {
            WilcardFileNameComparer wc = new WilcardFileNameComparer (pattern);
            Assert.False (wc.IsMatch (filename));
        }
    }
}