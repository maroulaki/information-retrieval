using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Queries.Mlt;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Lucene.Net.Util.Fst.Util;

namespace lucengine
{
    public class Searcher
    {
        public static IEnumerable<Document> SearchIndex(string q, int topNo)
        {
            Directory directory = null;
            if (Globals.useBM25 && Globals.BM25Dir != null)
            {
                directory = Globals.BM25Dir;
            } else if (!Globals.useBM25 && Globals.StandardDir != null) {

                directory = Globals.StandardDir;
            }

            if ((directory != null) && !String.IsNullOrEmpty(q))
            {
                using (DirectoryReader iReader = DirectoryReader.Open(directory))
                {
                    IndexSearcher iSearcher = new IndexSearcher(iReader);
                    QueryParser parser = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "content", Globals.Analyzer);
                    Query query = parser.Parse(q);
                    ScoreDoc[] hits = iSearcher.Search(query, null, topNo).ScoreDocs;
                    foreach (ScoreDoc hit in hits)
                    {
                        Document doc = iSearcher.Doc(hit.Doc);
                        doc.Add(new StringField("LuceneID", hit.Doc.ToString(), Field.Store.YES));
                        yield return doc;
                    }
                }
            } else
            {
                yield break;
            }
                    
        }

        public static IEnumerable<Document> MoreLikeThis(int ID)
        {
            Directory directory = Globals.useBM25 ? Globals.BM25Dir : Globals.StandardDir;
            if (directory == null) yield break;

            using (DirectoryReader iReader = DirectoryReader.Open(directory))
            {
                IndexSearcher iSearcher = new IndexSearcher(iReader);
                MoreLikeThis similar = new MoreLikeThis(iReader);
                similar.Analyzer = Globals.Analyzer;
                similar.FieldNames = new[] { "title", "authors", "abstract" };
                similar.MinTermFreq = 1;
                similar.MinDocFreq = 1;

                Query query = similar.Like(ID);
                ScoreDoc[] hits = iSearcher.Search(query, 10).ScoreDocs;

                foreach (ScoreDoc hit in hits)
                {
                    if (hit.Doc == ID) continue; // Skip original article

                    Document doc = iSearcher.Doc(hit.Doc);

                    doc.Add(new StringField("LuceneID", hit.Doc.ToString(), Field.Store.YES));
                    yield return doc;
                }
            }
        }
    }
}
