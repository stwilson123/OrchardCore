using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Abstractions.Datatransfer;
using SysMgt.BussnessDTOModule.SysGlobal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.Setup
{
    public class SetupTypeSearchModel 
    {
        public Page page { get; set; }     
    }
    public class SetupTypePageResult 
    {
        public string ID { get; set; }
        public string SetupTypeNo { get; set; }
        public string SetupTypeName { get; set; }
        public string SetupTypeValue { get; set; }
    }
    public class SetupTypeInfo 
    {
        public string ID { get; set; }
        public string SetupTypeNo { get; set; }
        public string SetupTypeName { get; set; }
        public string SetupTypeValue { get; set; }
        public List<SetupInfo> SetupList { get; set; }     
        public List<KeyValue> CodeList { get; set; }
    }

    /// <summary>
    /// 搜索使用
    /// </summary>
    public class SetupSearchModel 
    {        
        public Page page { get; set; }
        public string SetupType { get; set; }

    }

    /// <summary>
    /// 新增或编辑使用-只能Application层使用
    /// </summary>
    public class SetupInfo 
    {
        public string ID { get; set; }
        public string SetupNo { get; set; }
        public string SetupContents { get; set; }
        public string SetupParameter { get; set; }
       
    }

    public class SetupInfo4Delete 
    {
        public List<string> Ids { get; set; }
    }

    /// <summary>
    /// 分页返回的查询数据
    /// </summary>
    public class SetupPageResult 
    {
        public string ID { get; set; }
        public string SetupNo { get; set; }
        public string SetupContents { get; set; }
        public string SetupParameter { get; set; }     

    }
}
