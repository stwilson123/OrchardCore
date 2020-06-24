using System.Collections.Generic;
using BlocksCore.Abstractions.Data.Paging;

namespace BlocksCore.Data.Abstractions.Paging
{
    public class Group : IGroup
    {
        private string _groupOp;
        public string groupOp {
            set { _groupOp = value; }
            get { return IGroup.opend[_groupOp]; }
        }


        public IList<IRule> rules { set; get; } = new List<IRule>();

        public IList<IGroup> groups { set; get; } = new List<IGroup>();
    }
    
   


    public class Rule : IRule
    {
 
        public string field { get; set; }
        
        public string op { get; set; }

        public string data { get; set; }
    }
}