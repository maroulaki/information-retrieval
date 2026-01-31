using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Document = Lucene.Net.Documents.Document;

namespace lucengine
{
    public class Benchmark
    {
        // A class to story a query's (from query.txt) data
        public class BenchmarkQuery
        {
            public int ID { get; set; }
            public string text { get; set; }
        }

        public static List<BenchmarkQuery> LoadQueries()
        {
            List<BenchmarkQuery> queries = new List<BenchmarkQuery>();
            // What the Parser is currently reading on the line
            bool nowReading = false;

            // StringBuilder to hold the information
            StringBuilder currentQueryText = new StringBuilder();
            BenchmarkQuery currentQuery = null;

            // Loop over all documents
            foreach (string line in File.ReadLines("cacm/query.txt"))
            {
                if (line.StartsWith("."))
                {
                    // If new ID is encountered, last document is ready. Produce it and fetch new ID
                    if (line.StartsWith(".I"))
                    {
                        
                        if (currentQuery != null)
                        {
                            currentQuery.text = currentQueryText.ToString().Trim();
                            queries.Add(currentQuery);
                        }

                        // New query
                        currentQuery = new BenchmarkQuery();
                        currentQuery.ID = int.Parse(line.Substring(3).Trim());
                        currentQueryText.Clear();
                        nowReading = false;
                    }

                    // If query text is encountered, switch nowReading indicator to capture it
                    else if (line.StartsWith(".W")) nowReading = true;

                    // We don't care about the other fields
                    else nowReading = false;
                }
                else if (nowReading) 
                {
                    currentQueryText.AppendLine(line);
                }
            }

            // Produce last query
            if (currentQuery != null)
            {
                currentQuery.text = currentQueryText.ToString().Trim();
                queries.Add(currentQuery);
            }

            return queries;
        }

        public static Dictionary<int, HashSet<int>> LoadRelevant()
        {
            // I need a dictionary to hold all query IDs and a hashset of their relevant articles
            Dictionary<int, HashSet<int>> relevantDocs = new Dictionary<int, HashSet<int>>();

            foreach (string line in File.ReadLines("cacm/qrels_alt.txt"))
            {
                // Split each line to a table to get query ID and document ID's
                string[] line_parts = line.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                if (line_parts.Length < 3) continue;
                int queryID = int.Parse(line_parts[0]);
                int docID = int.Parse(line_parts[1]);

                if (!relevantDocs.ContainsKey(queryID)) relevantDocs[queryID] = new HashSet<int>();

                relevantDocs[queryID].Add(docID);
            }
            return relevantDocs;
        }

        public static void Evaluate()
        {
            List<BenchmarkQuery> queries = LoadQueries();
            Dictionary<int, HashSet<int>> qrels = LoadRelevant();
            StringBuilder csv = new StringBuilder();

            // Adding the header
            csv.AppendLine("queryID,Recall,Precision");

            foreach (BenchmarkQuery query in queries)
            {
                // If no relevant docs are given, skip
                if (!qrels.ContainsKey(query.ID)) continue;
                
                // Get the relevant documents for each query
                HashSet<int> relevantDocs = qrels[query.ID];
                int totalRel = relevantDocs.Count;
                List<Document> hits = Searcher.SearchIndex(query.text, 50);
                int relevant = 0;

                // First calculate precision and recall at each hit
                List<Tuple<double,double>> PR_points = new List<Tuple<double,double>>();
                for (int i = 0; i < hits.Count; i++)
                {
                    Document doc = hits[i];
                    int.TryParse(doc.Get("id"), out int docID);

                    if (relevantDocs.Contains(docID)) relevant++;
                    double precision = (double)relevant / (i + 1);
                    double recall = (double)relevant / totalRel;
                    PR_points.Add(new Tuple<double,double>(recall, precision));
                }

                // Then calculate 11-point interpolated precision
                for (int i = 0; i <= 10; i++)
                {
                    double targetRecall = i / 10.0;
                    double maxPrecision = 0.0;

                    foreach (var point in PR_points)
                    {
                        if (point.Item1 >= targetRecall)
                        {
                            if (point.Item2 >= maxPrecision) maxPrecision = point.Item2;
                        }
                    }

                    csv.AppendLine($"{query.ID},{targetRecall:F1},{maxPrecision:F4}");
                }
            }

            // Save results to a .csv file. Graphing will be done in excel
            string fileName = Globals.useBM25 ? "BM25_PR.csv" : "TFIDF_PR.csv";
            File.WriteAllText(fileName,csv.ToString());
        }
    }
}
