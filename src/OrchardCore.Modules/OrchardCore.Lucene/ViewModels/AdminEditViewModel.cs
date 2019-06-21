using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrchardCore.Lucene.ViewModels
{
    public class AdminEditViewModel
    {
        public string IndexName { get; set; }
        public string AnalyzerName { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Analyzers { get; set; }
    }
}
