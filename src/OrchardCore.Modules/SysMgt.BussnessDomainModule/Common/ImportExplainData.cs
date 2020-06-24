using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Common
{
    public class TableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表说明
        /// </summary>
        public string TableDesc { get; set; }

        /// <summary>
        /// 多个从表信息
        /// </summary>
        public List<TableInfo> SecondaryTables { get; set; }

        /// <summary>
        /// 单个从表信息
        /// </summary>
        //public TableInfo SecondaryTable { get; set; }

        /// <summary>
        /// 映射主表关系的列,映射从表关系的列。key主表，value从表
        /// </summary>
        public List<TableRelation> TableRelationColNames { get; set; } 

        ///// <summary>
        ///// 
        ///// </summary>
        //public string MapPrimaryColName { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public string MapSecondaryColName { get; set; }

        /// <summary>
        /// 列信息
        /// </summary>
        public List<ColumnAttribute> Columns { get; set; }
    }

    /// <summary>
    /// 表下列信息
    /// </summary>
    public class ColumnAttribute
    {
        /// <summary>
        /// 列名(数据表字段名）
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// EXCEL表列名
        /// </summary>
        public string ExcelColName { get; set; }
        /// <summary>
        /// 列描述
        /// </summary>
        public string ColDesc { get; set; }
        /// <summary>
        /// 导入时该列是否必须存在
        /// </summary>
        public bool IsColRequire { get; set; }
        /// <summary>
        /// 值要求唯一
        /// </summary>
        public bool IsDataUnique { get; set; }

        ///// <summary>
        ///// 列值是否允许空值
        ///// </summary>
        //public bool IsDataAllowNull { get; set; }

        /// <summary>
        /// 字段类型 是列在表字段的类型（当前支持三种类型string,datetime,number，其它类型会被认为是异常）
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 数据源类型:0-数据文件中直接读取（默认）；1-数据key在此XML文件中配置、value在数据文件中读取，以key值存入数据表  
        /// </summary>
        public int DataSourceType { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// 字段规则（正则表达式）
        /// </summary>
        public string DataValidRule { get; set; }

        /// <summary>
        /// 字段规则说明（正则表达式提示语）
        /// </summary>
        public string DataValidRuleDesc { get; set; }

        /// <summary>
        /// 映射表名
        /// </summary>
        public string MapTableName { get; set; }

        /// <summary>
        /// 映射EXCEL文件对应列名（动态列）
        /// </summary>
        public string MapValueColName { get; set; }

        /// <summary>
        /// 列关系信息（列有确定值）
        /// </summary>
        public List<ColumnRelation> MapValueColRelations { get; set; }
        ///// <summary>
        ///// 映射EXCEL文件对应列名
        ///// </summary>
        //public string MapValueColName { get; set; }

        /// <summary>
        /// 映射存储到数据库对应列名
        /// </summary>
        public string MapKeyColName { get; set; }

        /// <summary>
        /// dataSourceType=1适用 ，存储对应的列key和value
        /// </summary>
        public List<ColumnSourceSelectInfo> ColSelectList { get; set; } 

        //以下是数据处理时赋值的
        /// <summary>
        /// Excel列索引号
        /// </summary>
        public int? ExcelColIndex { get; set; }  

    }
    /// <summary>
    /// 列关系信息
    /// </summary>
    public class ColumnRelation
    {
        public string ColName { get; set; }
        public string DataType { get; set; }
        public string DataValue { get; set; }
    }

    /// <summary>
    /// 表关系信息及值
    /// </summary>
    public class TableRelation {

        public string PrimaryColName { get; set; }
        public string SecondaryColName { get; set; }
        public string PrimaryColValue { get; set; }
        public string DataType { get; set; }
    }

    /// <summary>
    /// 列源选择属性
    /// </summary>
    public class ColumnSourceSelectInfo
    {
        public string key { get; set; }
        public string value { get; set; }
    }

     
}
