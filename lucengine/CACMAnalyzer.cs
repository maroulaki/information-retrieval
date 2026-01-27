using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lucengine
{
    // A standard analyzer does not allow to config Porter stemming so we have to create a custom one
    public class CACMAnalyzer : Analyzer
    {
        private readonly CharArraySet stopWords;

        // The CACM analyzer is initialized by passing the stop words
        public CACMAnalyzer(CharArraySet stopWords)
        {
            this.stopWords = stopWords;
        }

        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            // We use standard tokenizer and account for lowercase/uppercase
            Tokenizer source = new StandardTokenizer(Lucene.Net.Util.LuceneVersion.LUCENE_48, reader);
            TokenStream stream = new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, source);

            // Common words are excluded
            stream = new StopFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, stream, stopWords);

            // Applying Porter stem filter
            stream = new PorterStemFilter(stream);
            return new TokenStreamComponents(source, stream);
        }

    }
}
