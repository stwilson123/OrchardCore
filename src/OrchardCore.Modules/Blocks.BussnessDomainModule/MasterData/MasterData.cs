using System;

namespace Blocks.BussnessDomainModule.MasterData
{
    public class MasterData
    {
        public string Id{ get; set; }
        public string tenancyName { get; set; }

        public string combobox { get; set; }

        public string city { get; set; }

        public bool isActive { get; set; }

        public string comment { get; set; }

        public DateTime registerTime { get; set; }

    }
}