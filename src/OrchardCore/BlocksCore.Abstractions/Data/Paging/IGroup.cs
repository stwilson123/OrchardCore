using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Data.Paging
{
    public interface IGroup
    {
        public static readonly Dictionary<string, string> opend = new Dictionary<string, string>()
        {

            { "AND" ,"&&"},{ "OR","||"},{"lt","<"},{"le","<="},{"gt",">"},{"ge",">="},{"bw","^"},{"bn","!^"},{"in","="},{"ni","!="},{"ew","|"},{"en","!@"},{"cn","~"},{"nc","!~"},{"nu","#"},{"nn","!#"},{ "bt","..."}
        };
        public IList<IRule> rules { get; }

        public IList<IGroup> groups {  get; }

        public string groupOp { get; }

    }
}
