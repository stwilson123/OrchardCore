using BlocksCore.Abstractions.Datatransfer;
using System;
using System.Collections.Generic;

namespace SysMgt.BussnessDTOModule.SysGlobal
{ 
    public class SysGlobalInfo 
    { 
        public string CKey { get; set; }
        public string Cid  { get; set; }
        public string OperateType { get; set; }
        public string HtmlString { get; set; }
        public List<KeyValue> ValueList { get; set; }
        public List<KeyValue> DateList { get; set; }
        public List<SelectValue> DDLList { get; set; }
    }
    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public List<KeyValue> SelectList { get; set; }
    }

    public class SelectValue
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public string Multiple { get; set; }
        public string AllowClear { get; set; }
        public string Url { get; set; }
        public List<KeyValue> SelectList { get; set; }
    }
}
