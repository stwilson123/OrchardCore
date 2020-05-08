using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Data.Paging
{
    public interface IRule
    {
        static readonly Dictionary<string, string> opend = new Dictionary<string, string>()
        {
            { "eq" ,"{0}=={1}"},{ "ne","{0}!={1}"},{"lt","{0}<{1}"},{"le","{0}<={1}"},{"gt","{0}>{1}"},{"ge","{0}>={1}"},{"bw","{0}.StartsWith{1}"},{"bn","!{0}.StartsWith{1}"},{"in","="},{"ni","!="},{"ew","{0}.EndsWith{1}"},{"en","!{0}.EndsWith{1}"},{"cn","{0}.Contains{1}"},{"nc","!{0}.Contains{1}"},{"nu","{0} == null"},{"nn","{0} != null"},{ "bt","..."}
        };
         string field { get; }

         string op { get; }

         string data { get; }
    }
}
