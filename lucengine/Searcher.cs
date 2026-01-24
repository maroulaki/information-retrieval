using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    for (int i = 0; i < hits.Length; i++)
                    {
                        yield return iSearcher.Doc(hits[i].Doc);
                    }
                }
            } else
            {
                yield break;
            }
                    
        }
    }
}
