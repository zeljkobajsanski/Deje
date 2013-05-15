using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Deje.Core.Model;
using Deje.Lucene.Analyzers;
using Deje.Lucene.Filters;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure;
using Directory = Lucene.Net.Store.Directory;
using System.Linq;

namespace Deje.Lucene
{
    public class Index
    {
        private readonly Directory m_Directory = new AzureDirectory(CloudStorageAccount.FromConfigurationSetting("AzureStorageConnectionString"), "indexes");

        private IndexSearcher m_Searcher;
        

        public void ObrisiIndekse()
        {
            var files = m_Directory.List();
            foreach (var file in files)
            {
                m_Directory.DeleteFile(file);
            }
        }

        public void IndeksirajArtikle(IEnumerable<Artikal> artikli)
        {
            var indexWriter = IndexWriter();

            try
            {
                foreach (var artikal in artikli)
                {
                    var document = new Document();
                    
                    document.Add(new Field("Tip", "a", Field.Store.NO, Field.Index.UN_TOKENIZED));
                    document.Add(new Field("Id", artikal.Id.ToString(), Field.Store.YES, Field.Index.NO));
                    document.Add(new Field("Naziv", artikal.Naziv, Field.Store.YES, Field.Index.TOKENIZED));
                    if (artikal.IdKategorijeArtikla.HasValue)
                    {
                        document.Add(new Field("IdKategorijeArtikla", artikal.IdKategorijeArtikla.ToString(), Field.Store.YES, Field.Index.NO));
                    }
                    if (artikal.IdSinonima.HasValue)
                    {
                        document.Add(new Field("IdSinonima", artikal.IdSinonima.ToString(), Field.Store.YES, Field.Index.NO));
                    }
                    if (artikal.IdDobavljaca.HasValue)
                    {
                        document.Add(new Field("IdDobavljaca", artikal.IdDobavljaca.ToString(), Field.Store.YES, Field.Index.NO));
                    }
                    if (artikal.Slika != null)
                    {
                        document.Add(new Field("Slika", artikal.Slika, Field.Store.YES, Field.Index.NO));
                    }
                    if (artikal.Opis != null)
                    {
                        document.Add(new Field("Opis", artikal.Opis, Field.Store.YES, Field.Index.NO));
                    }
                    document.Add(new Field("Cena", artikal.Cena.ToString("n2"), Field.Store.YES, Field.Index.NO));
                    document.Add(new Field("Aktivan", artikal.Aktivan.ToString(), Field.Store.YES, Field.Index.NO));
                    indexWriter.AddDocument(document);
                }
            }
            finally
            {
                indexWriter.Close();
            }
        }

        public void IndeksirajSinonime(IEnumerable<Sinonim> sinonimi)
        {
            var indexWriter = IndexWriter();
            try
            {
                foreach (var sinonim in sinonimi)
                {
                    var document = new Document();
                    document.Add(new Field("Tip", "s", Field.Store.NO, Field.Index.UN_TOKENIZED));
                    document.Add(new Field("Id", sinonim.Id.ToString(), Field.Store.YES, Field.Index.NO));
                    document.Add(new Field("Naziv", sinonim.Naziv, Field.Store.NO, Field.Index.TOKENIZED));

                    indexWriter.AddDocument(document);
                }
            }
            finally
            {
                indexWriter.Close();
            }
        }

        public IList<Artikal> PretraziArtikle(string naziv, bool fuzzy)
        {
            var searcher = IndexSearcher();
            if (searcher == null) throw new Exception("Pretraga indexa neuspešna");
            try
            {
                var reader = new StringReader(naziv);
                var tokenFilter =
                    new SerbianLatinTokenFilter(new LowerCaseFilter(new StandardFilter(new StandardTokenizer(reader))));
                var tokens = new List<string>();
                Token token;
                while ((token = tokenFilter.Next()) != null)
                {
                    tokens.Add(token.TermText());
                }
                var query = new BooleanQuery();
                query.Add(new TermQuery(new Term("Tip", "a")), BooleanClause.Occur.MUST);
                foreach (var t in tokens)
                {
                    var term = new Term("Naziv", t);
                    if (fuzzy)
                    {
                        query.Add(new FuzzyQuery(term), BooleanClause.Occur.MUST);
                    }
                    else
                    {
                        query.Add(new TermQuery(term), BooleanClause.Occur.MUST);
                    }
                }
                var hits = searcher.Search(query);
                var max = hits.Length();
                var artikli = new List<Artikal>();
                for (int i = 0; i < max; i++)
                {
                    var document = hits.Doc(i);
                    var artikal = new Artikal()
                    {
                        Id = int.Parse(document.GetField("Id").StringValue()),
                        Naziv = document.GetField("Naziv").StringValue(),
                        IdKategorijeArtikla = document.GetField("IdKategorijeArtikla") != null ? int.Parse(document.GetField("IdKategorijeArtikla").StringValue()) : (int?)null,
                        IdSinonima = document.GetField("IdSinonima") != null ? int.Parse(document.GetField("IdSinonima").StringValue()) : (int?)null,
                        IdDobavljaca = document.GetField("IdDobavljaca") != null ? int.Parse(document.GetField("IdDobavljaca").StringValue()) : (int?)null,
                        Slika = document.GetField("Slika") != null ? document.GetField("Slika").StringValue() : null,
                        Opis = document.GetField("Opis") != null ? document.GetField("Opis").StringValue() : null,
                        Cena = decimal.Parse(document.GetField("Cena").StringValue()),
                        Aktivan = bool.Parse(document.GetField("Aktivan").StringValue()),
                        Score = hits.Score(i)
                    };
                    artikli.Add(artikal);
                }
                return artikli;
            }
            finally
            {
                //searcher.Close();
            }
        } 

        public IList<Sinonim> PretraziSinonime(string naziv, bool fuzzy)
        {
            var searcher = IndexSearcher();
            if (searcher == null) throw new Exception("Pretraga indexa neuspešna");
            var reader = new StringReader(naziv);
            var tokenFilter =
                new SerbianLatinTokenFilter(new LowerCaseFilter(new StandardFilter(new StandardTokenizer(reader))));
            var tokens = new List<string>();
            Token token;
            while ((token = tokenFilter.Next()) != null)
            {
                tokens.Add(token.TermText());
            }
            var query = new BooleanQuery();
            query.Add(new TermQuery(new Term("Tip", "s")), BooleanClause.Occur.MUST);
            foreach (var t in tokens)
            {
                var term = new Term("Naziv", t);
                if (fuzzy)
                {
                    query.Add(new FuzzyQuery(term), BooleanClause.Occur.MUST);
                }
                else
                {
                    query.Add(new TermQuery(term), BooleanClause.Occur.MUST);
                }
            }
            var hits = searcher.Search(query);
            var max = hits.Length();
            var sinonimi = new List<Sinonim>();
            for (int i = 0; i < max; i++)
            {
                var document = hits.Doc(i);
                var id = int.Parse(document.GetField("Id").StringValue());
                sinonimi.Add(new Sinonim{Id = id, Score = hits.Score(i)});
            }
            return sinonimi;
        }

        private IndexWriter IndexWriter()
        {
            IndexWriter indexWriter = null;
            while (indexWriter == null)
            {
                try
                {
                    indexWriter = new IndexWriter(m_Directory,
                                                  new SynonymAnalyzer(new TextFileSynonymEngine("sinonimi.txt"), new string[0]),
                                                  !IndexReader.IndexExists(m_Directory));
                }
                catch (LockObtainFailedException)
                {
                    m_Directory.ClearLock("write.lock");
                }
            }
            return indexWriter;
        }

        private IndexSearcher IndexSearcher()
        {
            if (m_Searcher != null) return m_Searcher;
            var pokusaj = 1;
            while (m_Searcher == null && pokusaj <= 5)
            {
                try
                {
                    m_Searcher = new IndexSearcher(m_Directory);
                }
                catch
                {
                    Thread.Sleep(1000);
                    pokusaj++;
                }
            }
            return m_Searcher;
        }
    }
}