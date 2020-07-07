using System;

namespace Blocks.BussnessDTOModule
{
    public class PageResult 
    {
        public string Id { get; set; }
        
        public string tenancyName { get; set; }
        
        public string city { get; set; }
        
        public long isActive { get; set; }

        public string comment { get; set; }
        
        public string comboboxId { get; set; }

        public string comboboxText { get; set; }
        
        public DateTime registerTime { get; set; }
        public long num { get; set; }
    }
}