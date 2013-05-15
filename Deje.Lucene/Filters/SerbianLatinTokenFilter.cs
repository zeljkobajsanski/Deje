using System.Text;
using Lucene.Net.Analysis;

namespace Deje.Lucene.Filters
{
    public class SerbianLatinTokenFilter : TokenFilter
    {
        public SerbianLatinTokenFilter(TokenStream input) : base(input)
        {
        }

        public override Token Next()
        {
            var token = input.Next();
            if (token == null) return null;
            var tokenText = token.TermText();
            var sb = new StringBuilder(tokenText);
            sb = sb.Replace('š', 's');
            sb = sb.Replace('Š', 's');
            sb = sb.Replace("Đ", "dj");
            sb = sb.Replace("đ", "dj");
            sb = sb.Replace("č", "c");
            sb = sb.Replace("Č", "c");
            sb = sb.Replace("ć", "c");
            sb = sb.Replace("Ć", "c");
            sb = sb.Replace("ž", "z");
            sb = sb.Replace("Ž", "z");
            return new Token(sb.ToString(), token.StartOffset(), token.StartOffset() + sb.Length);
        }
    }
}