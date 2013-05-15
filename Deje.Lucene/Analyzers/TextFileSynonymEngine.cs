using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Deje.Lucene.Analyzers
{
    public class TextFileSynonymEngine : ISynonymEngine
    {
        private readonly List<ReadOnlyCollection<string>> m_SynonymGroups = new List<ReadOnlyCollection<string>>();

        public TextFileSynonymEngine(string textFile)
        {
            using (var r = File.OpenText(textFile))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    var synonyms = line.Split('|').ToList();
                    if (synonyms.Count > 0)
                    {
                        m_SynonymGroups.Add(new ReadOnlyCollection<string>(synonyms));
                    }    
                }
            }
        }

        public IEnumerable<string> GetSynonyms(string word)
        {
            //enumerate all the synonym groups
            foreach (var synonymGroup in m_SynonymGroups)
            {
                //if the word is a part of the group return 
                //the group as the results.
                if (synonymGroup.Contains(word))
                {
                    //gonna use a read only collection for security purposes
                    return synonymGroup;
                }
            }

            return null;
        }
    }
}