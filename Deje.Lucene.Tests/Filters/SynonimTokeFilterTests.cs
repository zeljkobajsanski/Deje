using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Deje.Lucene.Tests.Filters
{
    [TestClass]
    public class SynonimTokeFilterTests
    {
         [TestMethod]
        public void Test1()
         {
             //var synonymEngine = new Mock<ISynonymEngine>();
             //synonymEngine.Setup(x => x.GetSynonyms("Pizza")).Returns(new string[] {"Pica", "Pizza"});
             //synonymEngine.Setup(x => x.GetSynonyms("Capriciosa")).Returns(new string[] { "Kapriæoza", "Capriciosa" });

             //var r = new StringReader("Mini Pizza Capriciosa");
             //var ts = new WhitespaceTokenizer(r);
             //var stf = new SynonymFilter(ts, synonymEngine.Object);

             //Token token = null;
             //var tokens = new List<string>();

             //while ((token = stf.Next()) != null)
             //{
             //    tokens.Add(token.TermText());
             //}

             //Assert.AreEqual(5, tokens.Count);
             //Assert.AreEqual("Mini", tokens[0]);
             //Assert.AreEqual("Pica", tokens[2]);
             //Assert.AreEqual("Pizza", tokens[1]);
             //Assert.AreEqual("Kapriæoza", tokens[4]);
             //Assert.AreEqual("Capriciosa", tokens[3]);
         }
    }
}