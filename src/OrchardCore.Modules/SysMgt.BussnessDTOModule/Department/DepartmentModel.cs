
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule
{
    public class DepartmentSearchModel 
    {
        public Page page { get; set; }
    }

    public class DepartmentInfo 
    {

        public string ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }
    }

    public class DepartmentPageResult 
    {

        public string ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

    }



}