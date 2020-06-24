using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.ConfigFiles
{
    public class ConfigFilesData
    {
        public string Id { get; set; }
        //类型
        public string FileType { get; set; }
        //文件名        
        public string FileName { get; set; }
        //文件存放路径
        public string FilePath { get; set; }
        public string FileFunction { get; set; }
    }
}