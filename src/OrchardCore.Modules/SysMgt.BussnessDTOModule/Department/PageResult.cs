
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Application.Abstratctions.Datatransfer;

namespace SysMgt.BussnessDTOModule
{
    public class PageResult 
    {

        public string ID { get; set; }

        public string DepartmentNo { get; set; }

        public string DepartmentName { get; set; }

    }
}