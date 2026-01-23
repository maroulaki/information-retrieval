using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Lucene.Net.Analysis.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using System.Diagnostics.Eventing.Reader;

namespace lucengine
{
    public class Indexer
    {
        public static void CreateIndex()
        {
            // Create Directory
            if (Globals.useBM25 && Globals.BM25Dir != null)
            {
                Globals.BM25Dir.Dispose();
            }
            else if (!Globals.useBM25 && Globals.StandardDir != null)
            {
                Globals.StandardDir.Dispose();
            }
            RAMDirectory directory = new RAMDirectory();

            // Load common_words.txt
            StreamReader commonWordsFile = new StreamReader("cacm/common_words.txt");
            CharArraySet stopWordsSet = WordlistLoader.GetWordSet(commonWordsFile, Lucene.Net.Util.LuceneVersion.LUCENE_48);
            commonWordsFile.Close();

            // Initialize the analyzer and IndexWriter
            CACMAnalyzer analyzer = new CACMAnalyzer(stopWordsSet);
            Globals.Analyzer = analyzer;
            IndexWriterConfig config = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyzer);
            if (Globals.useBM25) {
                config.Similarity = new Lucene.Net.Search.Similarities.BM25Similarity();
            } 
            IndexWriter indexWriter = new IndexWriter(directory, config);

            // Adding documents
            foreach (Document doc in ParseCACM("cacm/cacm.all")) {
                indexWriter.AddDocument(doc);
            }
            indexWriter.Commit();

            // Saving Index
            if (Globals.useBM25)
            {
                Globals.BM25Dir = directory;
            } else {
                Globals.StandardDir = directory;
            }
           
        }

        private static IEnumerable<Document> ParseCACM(string path)
        {
            // What the Parser is currently reading on the line
            string nowReading = "Nothing";

            // The ID of the document we are working on
            string currentID = null;

            // StringBuilders to hold the information
            StringBuilder currentTitle = new StringBuilder();
            StringBuilder currentAuthors = new StringBuilder();
            StringBuilder currentAbstract = new StringBuilder();

            // Loop over all documents
            foreach (string line in File.ReadLines(path))
            {
                if (line.StartsWith("."))
                {
                    // If new ID is encountered, last document is ready. Produce it and fetch new ID
                    if (line.StartsWith(".I"))
                    {
                        if (currentID != null)
                        {
                            yield return CreateDocument(currentID, currentTitle.ToString(), currentAuthors.ToString(), currentAbstract.ToString());
                        }
                        currentID = line.Substring(2).Trim();
                        currentTitle.Clear();
                        currentAuthors.Clear();
                        currentAbstract.Clear();
                        nowReading = "Nothing";
                    }

                    // If title, authors or abstract are encountered, switch nowReading indicator to capture them
                    else if (line.StartsWith(".T")) nowReading = "Title";
                    else if (line.StartsWith(".A")) nowReading = "Authors";
                    else if (line.StartsWith(".W")) nowReading = "Abstract";

                    // We don't care about the other fields
                    else nowReading = "Ignore";
                }
                else 
                {
                    // Record the info in the StringBuilders
                     switch (nowReading) {
                        case "Title":
                            currentTitle.Append(line + " ");
                            break;
                        case "Authors":
                            currentAuthors.Append(line + " ");
                            break;
                        case "Abstract":
                            currentAbstract.Append(line + " ");
                            break;
                        case "Ignore":
                        case "Nothing":
                            break;
                     }
                }
            }

            // Produce last document
            if (currentID != null)
            {
                yield return CreateDocument(currentID, currentTitle.ToString(), currentAuthors.ToString(), currentAbstract.ToString());
            }
        }

        private static Document CreateDocument(string ID, string title, string authors, string abstractText)
        {
            Document doc = new Document();

            // Settings for fields
            FieldType textFieldType = new FieldType
            {
                IndexOptions = IndexOptions.DOCS_AND_FREQS_AND_POSITIONS_AND_OFFSETS,
                IsIndexed = true,
                IsTokenized = true,
                IsStored = true,
                StoreTermVectors = true,
                StoreTermVectorPositions = true,
                StoreTermVectorOffsets = true
            };
            
            //Creating fields
            Field idField = new StringField("id", ID, Field.Store.YES);
            Field titleField = new Field("title", title, textFieldType);
            Field authorsField = new Field("authors", authors, textFieldType);
            Field abstractField = new Field("abstract", abstractText, textFieldType);

            // Field for title + authors + abstract for searching (not stored)
            string allContent = $"{title} {authors} {abstractText}";
            Field allContentField = new TextField("content", allContent, Field.Store.NO);

            // Adding fields to document
            doc.Add(idField);
            doc.Add(titleField);
            doc.Add(authorsField);
            doc.Add(abstractField);
            doc.Add(allContentField);

            return doc;
        }


    }
}
