using System;
using System.Collections.Generic;
using Deje.Lucene.Analyzers;
using Lucene.Net.Analysis;

namespace Deje.Lucene.Filters
{
    public class SynonymFilter : TokenFilter
    {
        private Queue<Token> synonymTokenQueue
            = new Queue<Token>();

        public ISynonymEngine SynonymEngine { get; private set; }

        public SynonymFilter(TokenStream input, ISynonymEngine synonymEngine)
            : base(input)
        {
            if (synonymEngine == null)
                throw new ArgumentNullException("synonymEngine");

            SynonymEngine = synonymEngine;
        }

        public override Token Next()
        {
            // if our synonymTokens queue contains any tokens, return the next one.
            if (synonymTokenQueue.Count > 0)
            {
                return synonymTokenQueue.Dequeue();
            }

            //get the next token from the input stream
            Token t = input.Next();

            //if the token is null, then it is the end of stream, so return null
            if (t == null)
                return null;

            //retrieve the synonyms
            IEnumerable<string> synonyms = SynonymEngine.GetSynonyms(t.TermText());
            
            //if we don't have any synonyms just return the token
            if (synonyms == null)
            {
                return t;
            }

            //if we do have synonyms, add them to the synonymQueue, 
            // and then return the original token
            foreach (string syn in synonyms)
            {
                //make sure we don't add the same word 
                if ( ! t.TermText().Equals(syn))
                {
                    //create the synonymToken
                    Token synToken = new Token(syn, t.StartOffset(), t.EndOffset(), "<SYNONYM>");
                    
                    // set the position increment to zero
                    // this tells lucene the the synonym is 
                    // in the exact same location as the originating word
                    synToken.SetPositionIncrement(0);

                    //add the synToken to the synonyms queue
                    synonymTokenQueue.Enqueue(synToken);
                }
            }

            //after adding the syn to the queue, return the orginal token
            return t;
        }
    }
}
