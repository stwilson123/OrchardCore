//using Blocks.BussnessEntityModule;
//using FreeSql; 
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks; 

//namespace SysMgt.BussnessDomainModule.Common
//{
//    /// <summary>
//    /// 创建数据库实例
//    /// </summary>
//    public class SqlConnect
//    { 
//        private static string connStr =  ConfigurationManager.ConnectionStrings["Default"].ConnectionString; 
//         public static IFreeSql Instance
//        {
//            get
//            {
//                return new FreeSqlBuilder().UseConnectionString(DataType.Oracle, connStr).Build(); 
//            }
//        } 
//    }
//}
