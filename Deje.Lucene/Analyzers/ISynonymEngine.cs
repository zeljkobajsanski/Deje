using System.Collections.Generic;

namespace Deje.Lucene.Analyzers
{
    public interface ISynonymEngine
    {
        IEnumerable<string> GetSynonyms(string word);
    }
}
