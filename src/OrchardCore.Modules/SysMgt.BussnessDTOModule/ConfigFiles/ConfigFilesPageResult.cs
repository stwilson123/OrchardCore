using BlocksCore.Application.Abstratctions.Datatransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDTOModule.ConfigFiles
{
    public class ConfigFilesPageResult 
    {
        public string Id { get; set; }
        //类型
        public string FileType { get; set; }
        //文件名
        public string FileName { get; set; }
        //文件路径
        public string FilePath { get; set; }
        //功能说明
        public string FileFunction { get; set; }
        //创建人
        public string Creater { get; set; }
        //创建时间
        public DateTime? CreateDate { get; set; }
    }
}
