using System.Collections.Generic;
using System.IO;
using Deje.Lucene.Analyzers;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Deje.Lucene.Tests.Analyzers
{
    [TestClass]
    public class SynonimAnalyzerFixtures
    {
        [TestMethod]
        public void PljeskavicaTest()
        {
            //var se = new Mock<ISynonymEngine>();
            //se.Setup(x => x.GetSynonyms("pljeska")).Returns(new[]{"pljeskavica", "pljesak"});

            //var analyzer = new SynonymAnalyzer(se.Object, new string[0]);
            //var ts = analyzer.TokenStream("Naziv", new StringReader("Pljeska"));
            //var tokens = new List<Token>();
            //Token token;
            //while ((token = ts.Next()) != null)
            //{
            //    tokens.Add(token);
            //}
        }
    }
}