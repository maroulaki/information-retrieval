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
    public class CACMAnalyzer : Analyzer
    {
        private readonly CharArraySet stopWords;
        public CACMAnalyzer(CharArraySet stopWords)
        {
            this.stopWords = stopWords;
        }

        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            Tokenizer source = new StandardTokenizer(Lucene.Net.Util.LuceneVersion.LUCENE_48, reader);
            TokenStream stream = new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, source);
            stream = new StopFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, stream, stopWords);
            stream = new PorterStemFilter(stream);

            return new TokenStreamComponents(source, stream);
        }

    }
}
