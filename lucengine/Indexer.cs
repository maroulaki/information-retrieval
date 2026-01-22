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

namespace lucengine
{
    public class Indexer
    {
        public void createIndex(bool useBM25)
        {
            // Create Directory
            Lucene.Net.Store.Directory directory = new RAMDirectory();

            // Load common_words.txt
            var commonWordsFile = new StreamReader("../cacm/common_words.txt");
            CharArraySet stopWordsSet = WordlistLoader.GetWordSet(commonWordsFile, Lucene.Net.Util.LuceneVersion.LUCENE_48);
            commonWordsFile.Close();

            // Initialize the analyzer and IndexWriter
            CACMAnalyzer analyzer = new CACMAnalyzer(stopWordsSet);
            IndexWriterConfig config = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyzer);
            if (useBM25) {
                config.Similarity = new Lucene.Net.Search.Similarities.BM25Similarity();
            } 
            IndexWriter indexWriter = new IndexWriter(directory, config);

            // Configuring fields
            FieldType textFieldType = new FieldType
            {
                IndexOptions = IndexOptions.DOCS_AND_FREQS_AND_POSITIONS_AND_OFFSETS,

            };

        }
    }
}
