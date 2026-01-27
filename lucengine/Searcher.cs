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
        public static List<Document> SearchIndex(string q, int topNo)
        {
            // Load the index
            Directory directory = Globals.useBM25 ? Globals.BM25Dir : Globals.StandardDir;

            if ((directory == null) || String.IsNullOrEmpty(q)) return null;

            List<Document> resultDocs = new List<Document>();
            using (DirectoryReader iReader = DirectoryReader.Open(directory))
            {
                // Create a searcher
                IndexSearcher iSearcher = new IndexSearcher(iReader);
                if (Globals.useBM25)
                {
                    iSearcher.Similarity = new Lucene.Net.Search.Similarities.BM25Similarity();
                }
                else
                {
                    iSearcher.Similarity = new Lucene.Net.Search.Similarities.DefaultSimilarity();
                }
                
                // Parse the query
                QueryParser parser = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "content", Globals.Analyzer);
                Query query;
                try
                {
                    query = parser.Parse(q);
                }
                catch (ParseException)
                {
                    query = parser.Parse(QueryParser.Escape(q));
                }

                // Get the hits and store them in a list to return, along with internal ID as a field. It is needed for "MoreLikeThis.Like(ID)" to work
                ScoreDoc[] hits = iSearcher.Search(query, null, topNo).ScoreDocs;
                foreach (ScoreDoc hit in hits)
                {
                    Document doc = iSearcher.Doc(hit.Doc);
                    doc.Add(new StringField("LuceneID", hit.Doc.ToString(), Field.Store.YES));
                    resultDocs.Add(doc);
                }
                return resultDocs;
            }       
        }

        public static List<Document> MoreLikeThis(int ID)
        {
            // Load the index
            Directory directory = Globals.useBM25 ? Globals.BM25Dir : Globals.StandardDir;
            if (directory == null) return null;

            using (DirectoryReader iReader = DirectoryReader.Open(directory))
            {
                // Make list for results and the searcher
                List<Document> similarDocs =  new List<Document>();
                IndexSearcher iSearcher = new IndexSearcher(iReader);
                if (Globals.useBM25)
                {
                    iSearcher.Similarity = new Lucene.Net.Search.Similarities.BM25Similarity();
                }
                else
                {
                    iSearcher.Similarity = new Lucene.Net.Search.Similarities.DefaultSimilarity();
                }

                // Create the object to fetch similar docs, based on all three relevant fields
                MoreLikeThis similar = new MoreLikeThis(iReader);
                similar.Analyzer = Globals.Analyzer;
                similar.FieldNames = new[] { "title", "authors", "abstract" };
                similar.MinTermFreq = 1;
                similar.MinDocFreq = 1;

                // I will be fetching the first 10 similar docs each time "More like this" is pressed
                Query query = similar.Like(ID);
                ScoreDoc[] hits = iSearcher.Search(query, 10).ScoreDocs;

                // Save similar docs in a list (along with internal ID's) an return them
                foreach (ScoreDoc hit in hits)
                {
                    if (hit.Doc == ID) continue; // Skip original article
                    Document doc = iSearcher.Doc(hit.Doc);
                    doc.Add(new StringField("LuceneID", hit.Doc.ToString(), Field.Store.YES));
                    similarDocs.Add(doc);
                }
                return similarDocs;
            }
        }
    }
}
