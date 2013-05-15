using System.IO;
using Deje.Lucene.Filters;
using Lucene.Net.Analysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Deje.Lucene.Tests.Filters
{
    [TestClass]
    public class SerbianLetterTokenFilterFixtures
    {
         [TestMethod]
        public void Test()
         {
             var tf = new SerbianLatinTokenFilter(new WhitespaceTokenizer(new StringReader("ŠšĐđČčĆćŽž")));
             var token = tf.Next();
             Assert.AreEqual("ssdjdjcccczz", token.TermText());
         }

        [TestMethod]
        public void UpperCase()
        {
            var tf = new SerbianLatinTokenFilter(new LowerCaseFilter(new WhitespaceTokenizer(new StringReader("ĐEVREK"))));
            var token = tf.Next();
            Assert.AreEqual("djevrek", token.TermText());
        }
    }
}