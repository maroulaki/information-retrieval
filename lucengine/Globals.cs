using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lucengine
{
    public class Globals
    {
        public static bool useBM25 { get; set; }
        public static RAMDirectory StandardDir { get; set; }
        public static RAMDirectory BM25Dir { get; set; }
        public static CACMAnalyzer Analyzer { get; set; }
    }
}
