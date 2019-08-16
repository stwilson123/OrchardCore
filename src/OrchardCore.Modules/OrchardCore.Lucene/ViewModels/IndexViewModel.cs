using System;
using System.Collections.Generic;

namespace OrchardCore.Lucene.ViewModels
{
    public class IndexViewModel
    {
        public string Name { get; set; }
        public string AnalyzerName { get; set; }
        public DateTime LastUpdateUtc { get; set; }

        // public IndexingStatus IndexingStatus { get; set; }
    }
}
