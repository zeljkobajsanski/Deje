using Deje.Lucene.Filters;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace Deje.Lucene.Analyzers
{
    public class SynonymAnalyzer : Analyzer
    {
        public ISynonymEngine SynonymEngine { get; private set; }

        private string[] m_StopWords = new string[0];

        public SynonymAnalyzer(ISynonymEngine engine, string[] stopWords)
        {
            SynonymEngine = engine;
            m_StopWords = stopWords;
        }

        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            //create the tokenizer
            TokenStream result = new StandardTokenizer(reader);

            //add in filters
            result = new StandardFilter(result); // first normalize the StandardTokenizer
            result = new LowerCaseFilter(result);// makes sure everything is lower case
            result = new SerbianLatinTokenFilter(result);
            result = new StopFilter(result, m_StopWords); // use the default list of Stop Words, provided by the StopAnalyzer class.
            result = new SynonymFilter(result, SynonymEngine); // injects the synonyms. 

            //return the built token stream.
            return result;
        }
    }
}
