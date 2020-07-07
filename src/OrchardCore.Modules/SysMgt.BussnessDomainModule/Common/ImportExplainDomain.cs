//using Blocks.BussnessEntityModule;
//using BlocksCore.Domain.Abstractions.Domain;
//using BlocksCore.Domain.Abstractions;
//using Microsoft.Extensions.Localization;
//using BlocksCore.Abstractions.Security; 
//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel; 
//using SysMgt.BussnessDTOModule.Common;
//using SysMgt.BussnessRespositoryModule.ConfigFiles; 
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Web;
//using System.Xml.Linq;

//namespace SysMgt.BussnessDomainModule.Common
//{
//    //public class ImportExplainDomain : IDomainService
//    //{
//    //    public IUserContext UserContext { get; set; }
//    //    public IConfigFilesRepository ConfigFilesRepository { get; set; }

//    //    /// <summary>
//    //    /// 数据库实例
//    //    /// </summary>
//    //    public IFreeSql freeSql = SqlConnect.Instance;

//    //    /// <summary>
//    //    /// 20分钟自动提交事务
//    //    /// </summary>
//    //    public TimeSpan timeSpanSubmit = new TimeSpan(0, 20, 0);

//    //    /// <summary>
//    //    /// 一次批量插入数据条数
//    //    /// </summary>
//    //    public int BatchInsertCount = 100;

//    //    /// <summary>
//    //    /// EXCEL导入的XML配置文件路径。文件服务器上的导入XML配置文件的地址
//    //    /// </summary>
//    //    private string ImportFileXMLPath = HttpContext.Current.Server.MapPath("/") + @"ConfigManager/ImportConfig/";

//    //    /// <summary>
//    //    /// 上传的EXCEL数据文件路径
//    //    /// </summary>
//    //    private string ImportFileExcelPath = HttpContext.Current.Server.MapPath("/") + @"ConfigManager/ImportConfig/";

//    //    ///// <summary>
//    //    ///// EXCEL模板文件所在服务器路径
//    //    ///// </summary>
//    //    //private string ImportExcelTemplatePath = @"../../Blocks.BussnessWebModule/ConfigManager/ImportConfig/";

//    //    public ImportExplainDomain(IUserContext userContext, IConfigFilesRepository configFilesRepository)
//    //    {
//    //        this.UserContext = userContext;
//    //        this.ConfigFilesRepository = configFilesRepository;
//    //    }

//    //    /// <summary>
//    //    /// 根据唯一KEY，到文件管理中找到导入的EXCEL模板地址
//    //    /// </summary>
//    //    /// <param name="pInfo">pInfo.ID EXCEL模板文件名称,不包括扩展名 例如：Sample.xlsx 那么 pInfo.ID="Sample"</param>
//    //    /// <returns>返回文件所在服务器路径</returns>
//    //    public string GetImportConfigInfo(CommonEntity pInfo)
//    //    {
//    //        #region 入参校验
//    //        if (pInfo == null || string.IsNullOrEmpty(pInfo.ID))
//    //        {
//    //            throw new BlocksBussnessException("101", L["传入参数为空，无法指定导入模板"], null);
//    //        }
//    //        string pFileName = pInfo.ID;
//    //        #endregion
//    //        //FILE_TYPE 数据字典配置的导入类型；FILE_NAME功能名称
//    //        var excelTemplateInfo = ConfigFilesRepository.FirstOrDefault(t => t.FILE_TYPE == "dic1" && (t.FILE_NAME == pFileName + @".xls" || t.FILE_NAME == pFileName + @".xlsx"));
//    //        if (excelTemplateInfo == null)
//    //        {
//    //            throw new BlocksException(L["请先到配置管理中上传EXCEL模板文件"]);
//    //        }
//    //        //TODO：根据文件名称（约定的唯一KEY），查找文件管理中的文件路径（这里是EXCEL+按钮唯一KEY）            
//    //        string excelPath = ImportFileExcelPath + excelTemplateInfo.FILE_NAME;
//    //        //string 
//    //        if (!File.Exists(excelPath))
//    //        {
//    //            throw new BlocksException(L("指定目录：{0}下文件不存在", excelPath));
//    //        }
//    //        return excelTemplateInfo.FILE_PATH;//@"../../Blocks.BussnessWebModule/ConfigManager/ImportConfig/";
//    //    }

//    //    /// <summary>
//    //    /// 执行从Excel中导入数据到表中
//    //    /// </summary>
//    //    /// <param name="pInfo">pInfo.ID 约定的XML的文件名称（不包括文件类型）。例如：Sample.XML。那么pFileName值为Sample</param>
//    //    /// <returns>导入结果，成功返回200，失败返回101及失败原因</returns>
//    //    public string ImportData(CommonEntity pInfo)
//    //    {
//    //        try
//    //        {
//    //            #region 入参校验
//    //            if (pInfo == null || string.IsNullOrEmpty(pInfo.ID))
//    //            {
//    //                throw new BlocksBussnessException("101", L["传入参数缺失，无法指定配置文件"], null);
//    //            }
//    //            string pFileName = pInfo.ID;

//    //            if (string.IsNullOrEmpty(pInfo.STR))
//    //            {
//    //                throw new BlocksBussnessException("101", L["传入参数缺失，无法查找上传的EXCEL数据文件"], null);
//    //            }
//    //            #endregion

//    //            #region 配置文件XML解析  
//    //            //TODO:FILE_TYPE需要预先指定好，然后从xmlInfo里读取绝对路径，这里需要修改 2019-12-05 Linda
//    //            var xmlInfo = ConfigFilesRepository.FirstOrDefault(t => t.FILE_TYPE == "dic1" && (t.FILE_NAME == pFileName + @".xml"));
//    //            if (xmlInfo == null)
//    //            {
//    //                throw new BlocksException(L["请先到配置管理中上传XML文件"]);
//    //            }
//    //            //string ImportFileXMLPath = System.Web.HttpContext.Current.Server.MapPath("/") + @"ConfigManager/ImportConfig/";
//    //            string xmlPath = ImportFileXMLPath + pFileName + @".xml";
//    //            //获取主表信息
//    //            TableInfo tableInfo = GetImportConfigTableInfo(xmlPath);
//    //            #endregion

//    //            #region EXCEL文件数据内容读取 
//    //            string excelPath = pInfo.STR;
//    //            DataTable dtData = ExcelToDataTable(excelPath, true);//true第一行默认是标题，名称与XML配置的一样
//    //            if (dtData == null || dtData.Rows.Count <= 0)
//    //            {
//    //                throw new BlocksException(L["未读取到EXCEL中有效的数据信息"]);
//    //            }
//    //            #endregion

//    //            #region 匹配XML配置文件与EXCEL文件内容中的列（以XML中配置的必有列为准）  
//    //            Dictionary<string, List<ColumnAttribute>> matchColumnsDictionray = GetMatchColumns(tableInfo, dtData);
//    //            if (matchColumnsDictionray == null || matchColumnsDictionray.Count == 0)
//    //            {
//    //                throw new BlocksException(L["XML配置文件中的设置在EXCEL中找不到匹配列"]);
//    //            }
//    //            #endregion

//    //            #region 执行数据插入
//    //            CreateBatchInsertSqlAndExecute(tableInfo, matchColumnsDictionray,dtData);
//    //            #endregion  

//    //            return "导入成功";
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            throw new BlocksBussnessException("101", L(ex.Message), null);
//    //        }
//    //    }


//    //    #region XML解析    
//    //    /// <summary>
//    //    /// 解析XML配置的节点信息
//    //    /// </summary>
//    //    /// <param name="pFilePath">配置文件全路径</param>
//    //    /// <returns></returns>
//    //    public TableInfo GetImportConfigTableInfo(string pFilePath)
//    //    {
//    //        #region 配置文件校验 
//    //        if (!File.Exists(pFilePath))
//    //        {
//    //            throw new BlocksException(L("指定目录：{0}下文件不存在", pFilePath));
//    //        }
//    //        #endregion

//    //        #region 配置文件解析
//    //        try
//    //        {
//    //            TableInfo tableInfo = new TableInfo();   //存放解析的表信息 
//    //            //读取文件 
//    //            XDocument xmlDoc = XDocument.Load(pFilePath);
//    //            XElement root = xmlDoc.Root;
//    //            //根节点
//    //            if (!string.Equals(root.Name.ToString(), "importTemplate"))
//    //            {
//    //                throw new BlocksException(L(@"未读取到配置文件中的importTemplate节点"));
//    //            }

//    //            #region 主表解析
//    //            List<string> excelColNamesXML = new List<string>(); //用于校验XML配置中的Excel列名是否有重复
//    //            XElement elementTable = root.Element("primaryTable");
//    //            if (elementTable == null)
//    //            {
//    //                throw new BlocksException(L["未读取到配置文件中的primaryTable节点"]);
//    //            }
//    //            tableInfo = ExplainXMLTable(root, "primaryTable");
//    //            foreach (var col in tableInfo.Columns)
//    //            {
//    //                if (excelColNamesXML.Contains(col.ExcelColName))
//    //                {
//    //                    throw new BlocksException(L("配置文件中column节点属性excelColName存在相同的列名\"{0}\"", col.ExcelColName));
//    //                }
//    //                excelColNamesXML.Add(col.ExcelColName);
//    //            }
//    //            #endregion

//    //            #region 从表解析（多表导入才有此节点）
//    //            IEnumerable<XElement> secondaryTables = root.Elements("secondaryTable");
//    //            if (secondaryTables != null && secondaryTables.Count() > 0)
//    //            {
//    //                List<TableInfo> listSecondaryTables = new List<TableInfo>();
//    //                foreach (var perTable in secondaryTables)
//    //                {
//    //                    TableInfo tab = ExplainXMLTable(root, "secondaryTable");
//    //                    if (tab.TableRelationColNames == null || tab.TableRelationColNames.Count == 0)
//    //                    {
//    //                        throw new BlocksException(L(@"配置文件中secondaryTable节点下tableRelation子节点未有效配置"));
//    //                    }
//    //                    foreach (var col in tab.Columns)
//    //                    {
//    //                        if (excelColNamesXML.Contains(col.ExcelColName))
//    //                        {
//    //                            throw new BlocksException(L("配置文件中column节点属性excelColName存在相同的列名{0}", col.ExcelColName));
//    //                        }
//    //                        excelColNamesXML.Add(col.ExcelColName);
//    //                    }
//    //                    listSecondaryTables.Add(tab);
//    //                }
//    //                tableInfo.SecondaryTables = listSecondaryTables;
//    //            }

//    //            #endregion

//    //            return tableInfo;
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            throw new BlocksException(L(ex.Message));
//    //        }
//    //        #endregion
//    //    }
//    //    private TableInfo ExplainXMLTable(XElement root, string elementName)
//    //    {
//    //        TableInfo tableInfo = new TableInfo();
//    //        XElement elementTable = root.Element(elementName);//"primaryTable"  or "secondaryTable"       
//    //        if (elementTable.Attribute("tableName") == null || string.IsNullOrEmpty(elementTable.Attribute("tableName").Value)) //属性中配置表名
//    //        {
//    //            throw new BlocksException(L("配置文件中{0}节点属性tableName缺失或值为空", elementName));
//    //        }
//    //        tableInfo.TableName = elementTable.Attribute("tableName").Value;
//    //        tableInfo.TableDesc = elementTable.Attribute("tableDesc") == null ? "" : elementTable.Attribute("tableDesc").Value.Trim();
//    //        List<TableRelation> listTableRelation = new List<TableRelation>();
//    //        IEnumerable<XElement> elementTableRelations = elementTable.Elements("tableRelation");

//    //        foreach (var elementTableRelation in elementTableRelations)//从表配置与主表对应主外键关系
//    //        {
//    //            TableRelation relTable = new TableRelation();
//    //            var mapPrimaryColNames = elementTableRelation.Attribute("mapPrimaryColName");
//    //            if (mapPrimaryColNames == null || string.IsNullOrEmpty(mapPrimaryColNames.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件中{0}节点中节点tableRelation对应的属性mapPrimaryColName缺失或值为空", elementName));
//    //            }
//    //            relTable.PrimaryColName = mapPrimaryColNames.Value.TrimEnd();

//    //            var mapSecondaryColNames = elementTableRelation.Attribute("mapSecondaryColName");
//    //            if (mapSecondaryColNames == null || string.IsNullOrEmpty(mapSecondaryColNames.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件中{0}节点中节点tableRelation对应的属性mapSecondaryColName缺失或值为空", elementName));
//    //            }
//    //            relTable.SecondaryColName = mapSecondaryColNames.Value.Trim();

//    //            var dataType = elementTableRelation.Attribute("dataType");
//    //            if (dataType == null || string.IsNullOrEmpty(dataType.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件中{0}节点中节点tableRelation对应的属性dataType缺失或值为空", elementName));
//    //            }
//    //            var dataTypeValue = dataType.Value.ToLower();
//    //            if (!dataTypeValue.Equals("number") && !dataTypeValue.Equals("string"))
//    //            {
//    //                throw new BlocksException(L("配置文件中{0}节点中节点tableRelation对应的属性dataType值未能识别", elementName));
//    //            }
//    //            relTable.DataType = dataTypeValue;
//    //            listTableRelation.Add(relTable);
//    //        }
//    //        tableInfo.TableRelationColNames = listTableRelation;

//    //        //解析表中的列 
//    //        List<ColumnAttribute> listColumn = new List<ColumnAttribute>();
//    //        List<string> listColName = new List<string>();
//    //        List<string> listExcelColName = new List<string>();
//    //        if (elementTable.Elements().Count() == 0 || elementTable.Elements("column").Count() == 0)
//    //        {
//    //            throw new BlocksException(L("配置文件中{0}节点下未配置column节点信息", elementName));
//    //        }
//    //        foreach (XElement col in elementTable.Elements())
//    //        {
//    //            if (!string.Equals(col.Name.ToString(), "column")) continue;
//    //            //获取列字段信息
//    //            ColumnAttribute colInfo = new ColumnAttribute();
//    //            var colName = col.Attribute("colName");
//    //            if (colName == null || string.IsNullOrEmpty(colName.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失colName属性或值为空", elementName));
//    //            }
//    //            colInfo.ColName = colName.Value.Trim();
//    //            if (listColName.Contains(colInfo.ColName))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失colName已经包含属性值{1}", elementName, colInfo.ColName));
//    //            }
//    //            listColName.Add(colInfo.ColName);

//    //            //获取列excel信息
//    //            var excelColName = col.Attribute("excelColName");
//    //            if (excelColName == null || string.IsNullOrEmpty(excelColName.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失excelColName属性或值为空", elementName));
//    //            }
//    //            colInfo.ExcelColName = excelColName.Value.Trim();
//    //            if (listColName.Contains(colInfo.ExcelColName))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失excelColName已经包含属性值{1}", elementName, colInfo.ExcelColName));
//    //            }
//    //            listExcelColName.Add(colInfo.ExcelColName);

//    //            //获取列备注信息
//    //            colInfo.ColDesc = col.Attribute("colDesc") == null ? "" : col.Attribute("colDesc").Value.Trim();

//    //            //列字段不能为空信息
//    //            var isColRequire = col.Attribute("isColRequire");
//    //            if (isColRequire == null || string.IsNullOrEmpty(isColRequire.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失isColRequire属性或值为空", elementName));
//    //            }
//    //            if (!isColRequire.Value.ToLower().Equals("true") && !isColRequire.Value.ToLower().Equals("false"))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失isColRequire属性值设置无效", elementName));
//    //            }
//    //            colInfo.IsColRequire = bool.Parse(isColRequire.Value.Trim());

//    //            //列唯一信息
//    //            var isDataUnique = col.Attribute("isDataUnique");
//    //            if (isDataUnique == null || string.IsNullOrEmpty(isDataUnique.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失isDataUnique属性或值为空", elementName));
//    //            }
//    //            if (!isDataUnique.Value.Trim().ToLower().Equals("true") && !isDataUnique.Value.Trim().ToLower().Equals("false"))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失isDataUnique属性值设置无效", elementName));
//    //            }
//    //            colInfo.IsDataUnique = bool.Parse(isDataUnique.Value.Trim());

//    //            //列字段数据类型（表字段类型）
//    //            var dataType = col.Attribute("dataType");
//    //            if (dataType == null || string.IsNullOrEmpty(dataType.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失dataType属性或值为空", elementName));
//    //            }
//    //            string dataTypeValue = dataType.Value.Trim().ToLower();
//    //            if (!dataTypeValue.Equals("number") && !dataTypeValue.Equals("string") && !dataTypeValue.Equals("datetime"))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失dataType属性值设置无效", elementName));
//    //            }
//    //            colInfo.DataType = dataTypeValue;

//    //            //colInfo.DataSourceType: 数据源类型:0-数据文件中直接读取（默认）；1-从select节点读取；2-从数据表中读取映射列 (mapTableName(映射表名）、mapValueColName(映射EXCEL文件对应列名）、mapKeyColName（映射存储到数据库对应列名）三个属性必须设置)
//    //            var dataSourceType = col.Attribute("dataSourceType");
//    //            if (dataSourceType == null || string.IsNullOrEmpty(dataSourceType.Value))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失dataSourceType属性或值为空", elementName));
//    //            }
//    //            string dataSourceTypeValue = dataSourceType.Value.Trim();
//    //            if (!dataSourceTypeValue.Equals("0") && !dataSourceTypeValue.Equals("1") && !dataSourceTypeValue.Equals("2"))
//    //            {
//    //                throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失dataSourceType属性值设置无效", elementName));
//    //            }
//    //            colInfo.DataSourceType = int.Parse(dataSourceTypeValue);
//    //            if (colInfo.DataSourceType == 1)
//    //            {
//    //                var selects = col.Elements("select");
//    //                if (selects == null || selects.Count() == 0)
//    //                {
//    //                    throw new BlocksException(L("配置文件{0}节点下子节点column中缺失select节点", elementName));
//    //                }
//    //                List<ColumnSourceSelectInfo> colSelectList = new List<ColumnSourceSelectInfo>();
//    //                foreach (XElement select in selects)
//    //                {
//    //                    var key = select.Attribute("key");
//    //                    if (key == null || string.IsNullOrEmpty(key.Value))
//    //                    {
//    //                        throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失select节点中属性key未配置", elementName));
//    //                    }
//    //                    ColumnSourceSelectInfo item = new ColumnSourceSelectInfo();
//    //                    item.key = key.Value.Trim();
//    //                    item.value = select.Value.Trim();
//    //                    var hasKey = colSelectList.Where(t => t.key == item.key);
//    //                    if (hasKey != null && hasKey.Count() > 0)
//    //                    {
//    //                        throw new BlocksException(L("配置文件{0}节点下子节点column节点中缺失select节点中属性key配置了重复值", elementName));
//    //                    }
//    //                    colSelectList.Add(item);
//    //                }
//    //                colInfo.ColSelectList = colSelectList;
//    //            }
//    //            else if (colInfo.DataSourceType == 2)
//    //            {
//    //                //列信息读自的表名
//    //                var mapTableName = col.Attribute("mapTableName");
//    //                if (mapTableName == null || string.IsNullOrEmpty(mapTableName.Value))
//    //                {
//    //                    throw new BlocksException(L(@"配置文件{0}节点下子节点column节点中缺失属性mapTableName配置", elementName));
//    //                }
//    //                colInfo.MapTableName = mapTableName.Value.Trim();

//    //                //列信息读自的表名下的字段集合（用于查找）  
//    //                var mapValueColName = col.Attribute("mapValueColName");
//    //                if (mapValueColName == null || string.IsNullOrEmpty(mapValueColName.Value.Trim()))
//    //                {
//    //                    throw new BlocksException(L(@"配置文件{0}节点下子节点column节点中缺失属性mapValueColName配置", elementName));
//    //                }
//    //                string mapValueCol = mapValueColName.Value.Trim();
//    //                colInfo.MapValueColName = mapValueCol;

//    //                #region colRelation 节点解析
//    //                var colRelations = col.Elements("colRelation"); //不必有节点
//    //                if (colRelations != null && colRelations.Count() > 0)
//    //                {
//    //                    List<ColumnRelation> listColRel = new List<ColumnRelation>();
//    //                    foreach (var rel in colRelations)
//    //                    {
//    //                        ColumnRelation colR = new ColumnRelation();
//    //                        //colName属性
//    //                        var relColName = rel.Attribute("colName");
//    //                        if (relColName == null || string.IsNullOrEmpty(relColName.Value.Trim()))
//    //                        {
//    //                            throw new BlocksException(L(@"配置文件{0}节点下子节点column节点中子节点colRelation缺失属性colName配置", elementName));
//    //                        }
//    //                        colR.ColName = relColName.Value.Trim();
//    //                        //dataType属性
//    //                        var relDataType = rel.Attribute("dataType");
//    //                        if (relDataType == null || string.IsNullOrEmpty(relDataType.Value.Trim()))
//    //                        {
//    //                            throw new BlocksException(L(@"配置文件{0}节点下子节点column节点中子节点colRelation缺失属性dataType配置", elementName));
//    //                        }
//    //                        string relDataTypeValue = relDataType.Value.Trim().ToLower();
//    //                        if (!relDataTypeValue.Equals("number") && !relDataTypeValue.Equals("string"))
//    //                        {
//    //                            throw new BlocksException(L(@"配置文件{0}节点下子节点column节点中子节点colRelation属性dataType配置值未能识别", elementName));
//    //                        }
//    //                        colR.DataType = relDataType.Value.Trim();
//    //                        //colRelation节点值
//    //                        var relColValue = rel.Value.Trim();
//    //                        if (string.IsNullOrEmpty(relColValue))
//    //                        {
//    //                            throw new BlocksException(L(@"配置文件{0}节点下子节点column节点中子节点colRelation属性值不能为空", elementName));
//    //                        }
//    //                        colR.DataValue = relColValue;
//    //                        listColRel.Add(colR);
//    //                    }
//    //                    colInfo.MapValueColRelations = listColRel;
//    //                }
//    //                #endregion

//    //                //列信息读自表中字段（用于存入）
//    //                var mapKeyColName = col.Attribute("mapKeyColName");
//    //                if (mapKeyColName == null || string.IsNullOrEmpty(mapKeyColName.Value))
//    //                {
//    //                    throw new BlocksException(L(@"配置文件{0}节点下子节点column节点中缺失属性mapKeyColName配置", elementName));
//    //                }
//    //                colInfo.MapKeyColName = mapKeyColName.Value.Trim();
//    //            }

//    //            var dataLength = col.Attribute("dataLength");
//    //            colInfo.DataLength = (dataLength == null || string.IsNullOrEmpty(dataLength.Value)) ? 0 : int.Parse(dataLength.Value.Trim());

//    //            var dataValidRule = col.Attribute("dataValidRule");
//    //            colInfo.DataValidRule = dataValidRule == null ? "" : dataValidRule.Value.Trim();

//    //            var dataValidRuleDesc = col.Attribute("dataValidRuleDesc");
//    //            colInfo.DataValidRuleDesc = dataValidRuleDesc == null ? "" : dataValidRuleDesc.Value.Trim();

//    //            listColumn.Add(colInfo);
//    //        }
//    //        tableInfo.Columns = listColumn;
//    //        return tableInfo;
//    //    }
//    //    #endregion

//    //    #region EXCEL数据读入
//    //    /// <summary>  
//    //    /// 将excel导入到datatable  
//    //    /// </summary>  
//    //    /// <param name="pFilePath">excel路径</param>  
//    //    /// <param name="isFirstLineColumnName">第一行是否是列名</param>  
//    //    /// <returns>返回datatable</returns>  
//    //    public DataTable ExcelToDataTable(string pFilePath, bool isFirstLineColumnName)
//    //    {
//    //        DataTable dataTable = null;
//    //        FileStream fs = null;
//    //        DataColumn column = null;
//    //        DataRow dataRow = null;
//    //        IWorkbook workbook = null;
//    //        ISheet sheet = null;
//    //        IRow row = null;
//    //        ICell cell = null;
//    //        int startRow = 0;//数据起始行
//    //        try
//    //        {
//    //            if (!File.Exists(pFilePath))
//    //            {
//    //                throw new BlocksException(L("指定目录：{0}下文件不存在", pFilePath));
//    //            }
//    //            using (fs = File.OpenRead(pFilePath))
//    //            {
//    //                // 2007版本  
//    //                if (pFilePath.IndexOf(".xlsx") > 0)
//    //                    workbook = new XSSFWorkbook(fs);
//    //                // 2003版本  
//    //                else if (pFilePath.IndexOf(".xls") > 0)
//    //                    workbook = new HSSFWorkbook(fs);

//    //                if (workbook == null) return null;
//    //                sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
//    //                dataTable = new DataTable();
//    //                if (sheet == null) return null;

//    //                int rowCount = sheet.LastRowNum;//总行数  
//    //                if (rowCount <= 0) return null;

//    //                IRow firstRow = sheet.GetRow(0);//第一行  
//    //                int cellCount = firstRow.LastCellNum;//列数  

//    //                //构建datatable的列  
//    //                if (isFirstLineColumnName)
//    //                {
//    //                    startRow = 1;//如果第一行是列名，则从第二行开始读取
//    //                    List<string> listColNames = new List<string>();

//    //                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
//    //                    {
//    //                        cell = firstRow.GetCell(i);
//    //                        if (cell != null && cell.StringCellValue != null)
//    //                        {
//    //                            column = new DataColumn(cell.StringCellValue);
//    //                            if (dataTable.Columns.Contains(cell.StringCellValue))
//    //                            {
//    //                                throw new Exception(string.Format("EXCEL中列名\"{0}\"重复", cell.StringCellValue));
//    //                            }
//    //                            dataTable.Columns.Add(column);
//    //                        }
//    //                    }
//    //                }
//    //                else
//    //                {
//    //                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
//    //                    {
//    //                        column = new DataColumn("column" + (i + 1));
//    //                        dataTable.Columns.Add(column);
//    //                    }
//    //                }

//    //                //填充行  
//    //                for (int i = startRow; i <= rowCount; ++i)
//    //                {
//    //                    row = sheet.GetRow(i);
//    //                    if (row == null) continue;
//    //                    dataRow = dataTable.NewRow();
//    //                    for (int j = row.FirstCellNum; j < cellCount; ++j)
//    //                    {
//    //                        cell = row.GetCell(j);
//    //                        if (cell == null)
//    //                        {
//    //                            dataRow[j] = "";
//    //                            continue;
//    //                        }
//    //                        switch (cell.CellType) //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
//    //                        {
//    //                            case CellType.Blank:
//    //                                dataRow[j] = "";
//    //                                break;
//    //                            case CellType.Numeric:
//    //                                short format = cell.CellStyle.DataFormat;
//    //                                //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
//    //                                if (format == 14 || format == 31 || format == 57 || format == 58)
//    //                                    dataRow[j] = cell.DateCellValue;
//    //                                else
//    //                                    dataRow[j] = cell.NumericCellValue;
//    //                                break;
//    //                            case CellType.String:
//    //                                dataRow[j] = cell.StringCellValue;
//    //                                break;
//    //                        }
//    //                    }
//    //                    dataTable.Rows.Add(dataRow);
//    //                }
//    //            }
//    //            return dataTable;
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            if (fs != null)
//    //            {
//    //                fs.Close();
//    //            }
//    //            throw new BlocksException(L(ex.Message));
//    //            //  return null;
//    //        }
//    //    }
//    //    #endregion

//    //    #region XML和EXCEL匹配列
//    //    /// <summary>
//    //    /// 匹配XML配置文件与EXCEL文件内容中的列（以XML中配置的必有列为准）
//    //    /// </summary>
//    //    /// <param name="pTableInfo">XML表信息</param>
//    //    /// <param name="pDataColumnCollection">EXCEL中列信息</param>
//    //    /// <returns></returns>
//    //    public Dictionary<string, List<ColumnAttribute>> GetMatchColumns(TableInfo pTableInfo, DataTable dt)
//    //    {
//    //        Dictionary<string, List<ColumnAttribute>> matchColumnsDictionary = new Dictionary<string, List<ColumnAttribute>>();
//    //        DataColumnCollection pDataColumnCollection = dt.Columns;
//    //        List<ColumnAttribute> matchColumns = new List<ColumnAttribute>(); //全部有效的列存在这  
//    //        if (pTableInfo.Columns == null || pTableInfo.Columns.Count == 0) throw new BlocksException(L["primaryTable节点下columns节点不存在"]);

//    //        #region  primaryTable节点列信息
//    //        foreach (var col in pTableInfo.Columns)
//    //        {
//    //            bool hasColumn = false;
//    //            for (var i = 0; i < pDataColumnCollection.Count; i++)
//    //            {
//    //                if (col.ExcelColName.Equals(pDataColumnCollection[i].ColumnName, StringComparison.CurrentCultureIgnoreCase)) //匹配xml中和excel中的列
//    //                {
//    //                    col.ExcelColIndex = i;
//    //                    hasColumn = true;
//    //                    break;
//    //                }
//    //            }
//    //            if (!hasColumn && !col.IsColRequire) continue;//Excel中没找到，XML中列也不是必须的，不处理。                
//    //            if (!hasColumn && col.IsColRequire)//Excel中没找到，XML中列是必须的，异常
//    //            {
//    //                throw new BlocksException(L("Excel文件中缺失必须列{0}", col.ExcelColName));
//    //            }
//    //            matchColumns.Add(col);
//    //        }
//    //        matchColumnsDictionary.Add(pTableInfo.TableName, matchColumns);
//    //        if (pTableInfo.SecondaryTables == null || pTableInfo.SecondaryTables.Count == 0)
//    //        {
//    //            return matchColumnsDictionary;
//    //        }
//    //        #endregion

//    //        #region  secondaryTable 节点列信息 
//    //        foreach (var tab in pTableInfo.SecondaryTables)
//    //        {
//    //            matchColumns = new List<ColumnAttribute>();
//    //            foreach (var col in tab.Columns)
//    //            {
//    //                bool hasColumn = false;
//    //                for (var i = 0; i < pDataColumnCollection.Count; i++)
//    //                {
//    //                    if (col.ExcelColName.Equals(pDataColumnCollection[i].ColumnName, StringComparison.CurrentCultureIgnoreCase)) //匹配xml中和excel中的列
//    //                    {
//    //                        col.ExcelColIndex = i;
//    //                        hasColumn = true;
//    //                        break;
//    //                    }
//    //                }
//    //                if (!hasColumn && !col.IsColRequire) continue;//Excel中没找到，XML中列也不是必须的，不处理。                
//    //                if (!hasColumn && col.IsColRequire)//Excel中没找到，XML中列是必须的，异常
//    //                {
//    //                    throw new BlocksException(L("Excel文件中缺失必须列{0}", col.ExcelColName));
//    //                }
//    //                matchColumns.Add(col);
//    //            }
//    //            if (matchColumnsDictionary.ContainsKey(tab.TableName))
//    //            {
//    //                throw new BlocksException(L("XML文件中配置了相同的表{0}", tab.TableName));
//    //            }
//    //            matchColumnsDictionary.Add(tab.TableName, matchColumns);
//    //        }
//    //        #endregion

//    //        return matchColumnsDictionary;
//    //    }
//    //    #endregion

//    //    #region EXCEL数据根据XML唯一性要求配置的进行校验
//    //    /// <summary>
//    //    /// EXCEL数据根据XML唯一性要求配置的进行校验
//    //    /// </summary>
//    //    /// <param name="tableName">XML中配置的表名</param>
//    //    /// <param name="columns">XML中配置的列信息</param>
//    //    /// <param name="dt">待校验数据记录</param>
//    //    /// <param name="isRowIndexRemind">是否提示行号（默认false;若为true，要求dt中的行号跟EXCEL中行号一致时才用行号提示）</param>
//    //    public void CheckDataUnique(string tableName, List<ColumnAttribute> columns, DataTable dt, bool isRowIndexRemind = false)
//    //    {
//    //        var uniqueColumns = columns.Where(t => t.IsDataUnique == true);
//    //        if (uniqueColumns == null || uniqueColumns.Count() == 0) return;
//    //        int uniqueColumnCount = uniqueColumns.Count();
//    //        var listUniqueColumns = uniqueColumns.ToList();
//    //        string[] filter = new string[uniqueColumnCount];
//    //        string errorMsg = string.Empty;
//    //        DataTable dtNew = new DataTable();
//    //        for (int i = 0; i < uniqueColumnCount; i++)
//    //        {
//    //            DataColumn dtNewColumn = new DataColumn();
//    //            dtNewColumn.ColumnName = listUniqueColumns[i].ExcelColName;
//    //            dtNew.Columns.Add(dtNewColumn);
//    //        }

//    //        for (int i = 0; i < dt.Rows.Count; i++)
//    //        {
//    //            for (int j = 0; j < dtNew.Rows.Count; j++)
//    //            {
//    //                int matchCount = 0;
//    //                errorMsg = string.Empty;
//    //                foreach (DataColumn col in dtNew.Columns)
//    //                {
//    //                    if (dt.Rows[i][col.ColumnName].ToString().Equals(dtNew.Rows[j][col.ColumnName].ToString()))
//    //                    {
//    //                        matchCount++;
//    //                        errorMsg += string.Format("列\"{0}\"的值\"{1}\"，", col.ColumnName, dt.Rows[i][col.ColumnName].ToString());
//    //                    }
//    //                    if (matchCount == dtNew.Columns.Count)
//    //                    {
//    //                        if (isRowIndexRemind)
//    //                        {
//    //                            throw new BlocksException(L("校验表{0}--第{1}行数据重复", tableName, i + 1), null);
//    //                        }
//    //                        throw new BlocksException(L("校验表{0}--{1}的数据重复", tableName, errorMsg.TrimEnd('，')), null);
//    //                    }
//    //                }
//    //            }
//    //            DataRow dr = dtNew.NewRow();
//    //            foreach (DataColumn col in dtNew.Columns)
//    //            {
//    //                dr[col.ColumnName] = dt.Rows[i][col.ColumnName];
//    //            }
//    //            dtNew.Rows.Add(dr);
//    //        }
//    //    }
//    //    #endregion

//    //    #region 数据校验及导入

//    //    public void CreateBatchInsertSqlAndExecute(TableInfo tableInfo, Dictionary<string, List<ColumnAttribute>> matchColumnsDictionray, DataTable dtData)
//    //    { 
//    //        var primaryMatchColumns = matchColumnsDictionray.FirstOrDefault(t => t.Key == tableInfo.TableName).Value;
//    //        if (matchColumnsDictionray.Count == 1) //认为只有主表 
//    //        {
//    //            //数据导入并执行sql
//    //            freeSql.Transaction(() => {
//    //                CreateBatchInsertSqlAndExecute4AllRows(tableInfo, primaryMatchColumns, dtData);
//    //            }, timeSpanSubmit); 
//    //            return;
//    //        }

//    //        #region 当存在主从表时，主表数据需要去重
//    //        string[] filter = new string[primaryMatchColumns.Count];
//    //        for (int i = 0; i < primaryMatchColumns.Count; i++)
//    //        {
//    //            filter[i] = primaryMatchColumns[i].ExcelColName;
//    //        }
//    //        //主表xml配置的所有列进行分组(去重复记录）
//    //        DataTable dtPrimary = new DataTable();
//    //        DataView dv = dtData.DefaultView;
//    //        dtPrimary = dv.ToTable(tableInfo.TableName, true, filter); //dv.ToTable(true, new string[] { "id", "name", "code" })                                                           
                                                                      
//    //        freeSql.Transaction(() =>
//    //        { 
//    //            //主表处理
//    //            DataTable processDtPrimary = CreateBatchInsertSqlAndExecute4PrimaryTable(tableInfo, primaryMatchColumns, dtPrimary);
//    //            foreach (var tab in tableInfo.SecondaryTables)
//    //            {
//    //                var secondMatchColumns = matchColumnsDictionray.FirstOrDefault(t => t.Key == tab.TableName).Value;
//    //                //从表处理
//    //                CreateBatchInsertSqlAndExecute4AllRows(tab, secondMatchColumns, dtData, processDtPrimary);
//    //            }
//    //        }, timeSpanSubmit);
//    //        #endregion  
//    //    }
    
//    //    /// <summary>
//    //    /// EXCEL行记录插入到数据表一对一情况，适用于（仅有主表或者主从结构的从表插入sql构造和执行）
//    //    /// 创建批量插入到Oracle数据库的SQL语句
//    //    /// 创建人CREATER和创建时间CREATEDATE两列会默认构造当前插入的时间和登录用户。
//    //    /// </summary>
//    //    /// <param name="tableName">表名</param>
//    //    /// <param name="matchColumns">列信息</param>
//    //    /// <param name="dt">数据信息</param>
//    //    /// <returns>sql语句</returns>
//    //    private string CreateBatchInsertSqlAndExecute4AllRows(TableInfo tableInfo, List<ColumnAttribute> matchColumns, DataTable dt, DataTable dtPrimary = null)
//    //    {
//    //        //校验Excel中数据唯一性
//    //        CheckDataUnique(tableInfo.TableName, matchColumns, dt);
//    //        //用于存储返回的整体 sql
//    //        StringBuilder sb = new StringBuilder();
//    //        sb.Append(" insert all ");
//    //        string tableName = tableInfo.TableName;

//    //        #region 数据唯一性校验 
//    //        DataTable dtAllData = new DataTable();
//    //       bool hasDataUniqueProcess = false; //是否有唯一性要求列
//    //        var dataUniqueCol = matchColumns.Where(t => t.IsDataUnique == true);
//    //        if (dataUniqueCol != null && dataUniqueCol.Count() > 0)
//    //        {
//    //            hasDataUniqueProcess = true;
//    //            dtAllData= GetDataBaseData(tableInfo.TableName, dataUniqueCol);
//    //        }
            
//    //        #endregion

//    //        #region 事先把需要从表中关联的数据查出来 
//    //        matchColumns = GetColumnRelationData(tableInfo, matchColumns);//给需要从表中查询的列赋值key ，value 
//    //        #endregion

//    //        #region 构造SQL并执行
//    //        //列头信息
//    //        string strInsertColumnsSql = CreateColumnNameSql(tableName, matchColumns, tableInfo.TableRelationColNames); 

//    //        //处理数据信息
//    //        int tmpCount = 0;
//    //        for (var i = 0; i < dt.Rows.Count; i++)
//    //        {
//    //            DataRow dr=dt.Rows[i];
//    //            // 需要构造：when(not exists(select 1 from sys_third_system_type where id = '2')) then 
//    //            //sbDataUniqueSql 用来sql的where条件  eg  id = '2'
//    //            StringBuilder sbDataUniqueSql = new StringBuilder();

//    //            //sbDataSql 用来构造一条数据sql  eg: into sys_third_system_type(id,system_no,system_name,creater) values('2222','2222','xxx','1')                
//    //            StringBuilder sbDataSql = new StringBuilder();
//    //            sbDataSql.Append(strInsertColumnsSql);
//    //            sbDataSql.Append(" values ('" + Guid.NewGuid().ToString() + "',");//主键
//    //            foreach (var col in matchColumns)
//    //            {
//    //                string dataValue = dt.Rows[i][(int)col.ExcelColIndex].ToString().Trim();//数据
//    //                dataValue = CheckDataValue(dataValue, i, col);//数据值校验 
//    //                dr[col.ExcelColName] = dataValue; 

//    //                switch (col.DataType.Trim().ToLower())
//    //                {
//    //                    case "string":
//    //                        if (col.IsDataUnique)
//    //                        {
//    //                            sbDataUniqueSql.Append(string.Format(" {0}='{1}' and ", col.ColName, dataValue));
//    //                        }
//    //                        sbDataSql.Append("'" + dataValue + "',");
//    //                        break;
//    //                    case "datetime":
//    //                        if (col.IsDataUnique)
//    //                        {
//    //                            sbDataUniqueSql.Append(string.Format(" {0}=to_date('{1}', 'YYYY-MM-DD HH24:MI:SS') and ", col.ColName, dataValue));
//    //                        }
//    //                        sbDataSql.Append(string.Format(" to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'),", dataValue));
//    //                        break;
//    //                    case "number":
//    //                        if (col.IsDataUnique)
//    //                        {
//    //                            sbDataUniqueSql.Append(string.Format(" {0}={1} and ", col.ColName, decimal.Parse(dataValue)));
//    //                        }
//    //                        sbDataSql.Append(decimal.Parse(dataValue) + ",");
//    //                        break;
//    //                    default:
//    //                        throw new BlocksBussnessException("101", L("配置XML文件中列{0}的dataType属性值未能识别", col.ColName), null);
//    //                        // break;
//    //                }
//    //            }

//    //            //校验数据是否在数据库中已经存在 
//    //            CheckDataBaseExist(dr, i, dtAllData, dataUniqueCol); 

//    //            //有唯一数据列 
//    //            if (hasDataUniqueProcess)
//    //            {
//    //                string tmpDataUniqueSql = sbDataUniqueSql.ToString();
//    //                string strDataUniqueSql = string.Format(" when(not exists(select 1 from {0} where {1} )) then ", tableName, tmpDataUniqueSql.Substring(0, tmpDataUniqueSql.Length - 4));
//    //                sb.Append(strDataUniqueSql);
//    //            }
//    //            if (dtPrimary != null && dtPrimary.Rows.Count > 0)
//    //            {
//    //                //找到主表的ID
//    //                string primaryID = string.Empty;
//    //                int colValueMatchCount = 0;
//    //                for (var m = 0; m < dtPrimary.Rows.Count; m++)
//    //                {
//    //                    colValueMatchCount = 0;
//    //                    foreach (DataColumn pr in dtPrimary.Columns)
//    //                    {
//    //                        if (pr.ColumnName.Equals("ID")) continue;
//    //                        string valueSource = dtPrimary.Rows[m][pr.ColumnName].ToString().Trim();
//    //                        string valueDest = dt.Rows[i][pr.ColumnName].ToString().Trim();
//    //                        if (valueDest.Equals(valueSource))
//    //                        {
//    //                            colValueMatchCount++;
//    //                        }
//    //                    }
//    //                    if (colValueMatchCount == dtPrimary.Columns.Count - 1) // -1是去掉ID列
//    //                    {
//    //                        primaryID = dtPrimary.Rows[m]["ID"].ToString();
//    //                        if (string.IsNullOrEmpty(primaryID))
//    //                        {
//    //                            throw new BlocksBussnessException("101", L["主表唯一键不能为空"], null);
//    //                        }
//    //                        foreach (var tab in tableInfo.TableRelationColNames)
//    //                        {
//    //                            string dataVal = dtPrimary.Rows[m][tab.PrimaryColName].ToString();
//    //                            switch (tab.DataType)
//    //                            {
//    //                                case "number":
//    //                                    sbDataSql.Append(string.Format(" {0},", dataVal));
//    //                                    break;
//    //                                default://"string"
//    //                                    sbDataSql.Append(string.Format("'{0}',", dataVal));
//    //                                    break;
//    //                            }
//    //                        }
//    //                        break;
//    //                    }
//    //                }
//    //                if (colValueMatchCount < dtPrimary.Columns.Count - 1)
//    //                {
//    //                    throw new BlocksBussnessException("101", L["插入明细表数据时，没找到主表数据."], null);
//    //                }
//    //            }

//    //            //拼上默认两列创建人和创建时间
//    //            sbDataSql.Append(string.Format("to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'),'{1}')", DateTime.Now.ToString(), UserContext.GetCurrentUser().UserId));//to_date('2019-07-01 19:56:54','YYYY-MM-DD HH24:MI:SS')
//    //            string strDataSql = sbDataSql.ToString();
//    //            sb.Append(strDataSql);

//    //            #region 数据库操作
//    //            tmpCount++;
//    //            if (tmpCount == BatchInsertCount || i == dt.Rows.Count - 1)
//    //            {
//    //                sb.Append(" select 1 from dual ");
//    //                int successCount = freeSql.Ado.ExecuteNonQuery(sb.ToString());
//    //                if (successCount < tmpCount)
//    //                {
//    //                    throw new BlocksBussnessException("101", L["执行导入失败，数据与配置中列要求不符合"], null);
//    //                }
//    //                //重新赋值
//    //                sb = new StringBuilder();
//    //                sb.Append(" insert all ");
//    //                tmpCount = 0;
//    //            }
//    //            #endregion
//    //        }

//    //        #endregion
      
//    //        return "导入成功";
//    //    }

//    //    /// <summary>
//    //    /// 适用于主从表sql构造和执行
//    //    /// </summary>
//    //    /// <param name="tableName"></param>
//    //    /// <param name="matchColumns"></param>
//    //    /// <param name="dt">主表的插入数据</param>
//    //    private DataTable CreateBatchInsertSqlAndExecute4PrimaryTable(TableInfo tableInfo, List<ColumnAttribute> matchColumns, DataTable dt)
//    //    {
//    //        //主表已经去重了，不需要再校验唯一性
//    //        //CheckDataUnique(tableInfo.TableName, matchColumns, dt);

//    //        //用于存储返回的整体 sql
//    //        StringBuilder sb = new StringBuilder();
//    //        sb.Append(" insert all ");
//    //        string tableName = tableInfo.TableName;

//    //        #region 数据唯一性校验    
//    //        DataTable dtAllData = new DataTable();
//    //        bool hasDataUniqueProcess = false; //是否有唯一性要求列
//    //        var dataUniqueCol = matchColumns.Where(t => t.IsDataUnique == true);
//    //        if (dataUniqueCol != null && dataUniqueCol.Count() > 0)
//    //        {
//    //            hasDataUniqueProcess = true;
//    //            dtAllData = GetDataBaseData(tableInfo.TableName, dataUniqueCol);
//    //        }

//    //        #endregion

//    //        #region 事先把需要从表中关联的数据查出来 
//    //        matchColumns = GetColumnRelationData(tableInfo, matchColumns);//给需要从表中查询的列赋值key ，value 
//    //        #endregion

//    //        #region 构造SQL并执行
//    //        //列头信息构造
//    //        string strInsertColumnsSql = CreateColumnNameSql(tableName, matchColumns, tableInfo.TableRelationColNames);

//    //        //处理数据信息
//    //        //将ID添加到主表中，给后续从表关系ID使用
//    //        DataColumn colId = new DataColumn();
//    //        colId.ColumnName = "ID";
//    //        dt.Columns.Add(colId);

//    //        int tmpCount = 0;
//    //        DataTable dtCopy = dt.Copy();
//    //        for (var i = 0; i < dt.Rows.Count; i++)
//    //        {
//    //            DataRow dr = dtCopy.Rows[i];
//    //            string newGuid = Guid.NewGuid().ToString();
//    //            dt.Rows[i]["ID"] = newGuid;

//    //            // 需要构造：when(not exists(select 1 from sys_third_system_type where id = '2')) then 
//    //            //sbDataUniqueSql 用来sql的where条件  eg  id = '2'
//    //            StringBuilder sbDataUniqueSql = new StringBuilder();

//    //            //sbDataSql 用来构造一条数据sql  eg: into sys_third_system_type(id,system_no,system_name,creater) values('2222','2222','xxx','1')                
//    //            StringBuilder sbDataSql = new StringBuilder();
//    //            sbDataSql.Append(strInsertColumnsSql);
//    //            sbDataSql.Append(" values ('" + newGuid + "',");//主键
//    //            for (int c = 0; c < matchColumns.Count(); c++)
//    //            {
//    //                string dataValue = dt.Rows[i][c].ToString().Trim();//数据
//    //                var col = matchColumns[c];
//    //                #region 数据值校验 
//    //                //长度校验
//    //                if (col.DataLength > 0 && col.DataType.Trim().ToLower().Equals("string") && dataValue.Length > col.DataLength)
//    //                {
//    //                    throw new BlocksBussnessException("101", L("列{0}的数据{1}不能超过{2}字符", col.ColName, dataValue, col.DataLength), null);
//    //                }
//    //                //正则表达式校验 
//    //                if (!string.IsNullOrEmpty(col.DataValidRule) && !Regex.IsMatch(dataValue, col.DataValidRule))
//    //                {
//    //                    throw new BlocksBussnessException("101", L("列{0}数据值{1}有不符合{2}要求", col.ColName, dataValue, col.DataValidRuleDesc), null);
//    //                }
//    //                //数据类型值切换，从value替换到key存储
//    //                if (col.DataSourceType == 1 || col.DataSourceType == 2)
//    //                {
//    //                    var selInfo = col.ColSelectList.FirstOrDefault(t => t.value == dataValue);
//    //                    if (selInfo == null)
//    //                    {
//    //                        throw new BlocksBussnessException("101", L("列{0}，数据值{1}在XML配置文件中找不到对应列转换值", col.ColName, dataValue), null);
//    //                    }
//    //                    dataValue = selInfo.key;
//    //                    dr[col.ExcelColName] = dataValue; 
//    //                }
//    //                #endregion

//    //                switch (col.DataType.Trim().ToLower())
//    //                {
//    //                    case "string":
//    //                        if (col.IsDataUnique)
//    //                        {
//    //                            sbDataUniqueSql.Append(string.Format(" {0}='{1}' and ", col.ColName, dataValue));
//    //                        }
//    //                        sbDataSql.Append("'" + dataValue + "',");
//    //                        break;
//    //                    case "datetime":
//    //                        if (col.IsDataUnique)
//    //                        {
//    //                            sbDataUniqueSql.Append(string.Format(" {0}=to_date('{1}', 'YYYY-MM-DD HH24:MI:SS') and ", col.ColName, dataValue));
//    //                        }
//    //                        sbDataSql.Append(string.Format(" to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'),", dataValue));
//    //                        break;
//    //                    case "number":
//    //                        if (col.IsDataUnique)
//    //                        {
//    //                            sbDataUniqueSql.Append(string.Format(" {0}={1} and ", col.ColName, decimal.Parse(dataValue)));
//    //                        }
//    //                        sbDataSql.Append(decimal.Parse(dataValue) + ",");
//    //                        break;
//    //                    default:
//    //                        throw new BlocksBussnessException("101", L("配置XML文件中列{0}的dataType属性值未能识别", col.ColName), null);
//    //                        // break;
//    //                }
//    //            }

//    //            //校验数据是否在数据库中已经存在 
//    //            CheckDataBaseExist(dr, i, dtAllData, dataUniqueCol);

//    //            //有唯一数据列 
//    //            if (hasDataUniqueProcess)
//    //            {
//    //                string tmpDataUniqueSql = sbDataUniqueSql.ToString();
//    //                string strDataUniqueSql = string.Format(" when(not exists(select 1 from {0} where {1} )) then ", tableName, tmpDataUniqueSql.Substring(0, tmpDataUniqueSql.Length - 4));
//    //                sb.Append(strDataUniqueSql);
//    //            }
//    //            //拼上默认两列创建人和创建时间
//    //            sbDataSql.Append(string.Format("to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'),'{1}')", DateTime.Now.ToString(), UserContext.GetCurrentUser().UserId));//to_date('2019-07-01 19:56:54','YYYY-MM-DD HH24:MI:SS')
//    //            string strDataSql = sbDataSql.ToString();
//    //            sb.Append(strDataSql);

//    //            #region 数据库操作
//    //            tmpCount++;
//    //            if (tmpCount == BatchInsertCount || i == dt.Rows.Count - 1)
//    //            {
//    //                sb.Append(" select 1 from dual ");
//    //                int successCount = freeSql.Ado.ExecuteNonQuery(sb.ToString());
//    //                if (successCount < tmpCount)
//    //                {
//    //                    throw new BlocksBussnessException("101", L["执行导入失败，数据与配置中列要求不符合"], null);
//    //                }
//    //                //重新赋值
//    //                sb = new StringBuilder();
//    //                sb.Append(" insert all ");
//    //                tmpCount = 0;
//    //            }
//    //            #endregion
//    //        }
//    //        return dt;
//    //        #endregion
//    //    }

//    //    /// <summary>
//    //    /// 从数据库中捞取表中已有的数据
//    //    /// </summary>
//    //    /// <param name="tableName"></param>
//    //    /// <param name="matchColumns"></param>
//    //    /// <returns></returns>
//    //    private DataTable GetDataBaseData(string tableName, IEnumerable<ColumnAttribute> dataUniqueCol)
//    //    { 
//    //        StringBuilder sbAllDataSql = new StringBuilder();
//    //        sbAllDataSql.Append(" select ");
//    //        foreach (var uniqueCol in dataUniqueCol)
//    //        {
//    //            sbAllDataSql.Append(string.Format(" {0},", uniqueCol.ColName));
//    //        }
//    //        string allDataSql = sbAllDataSql.ToString();
//    //        allDataSql = string.Format("{0} from {1}", allDataSql.TrimEnd(','), tableName);
//    //        return freeSql.Ado.ExecuteDataTable(allDataSql); 
//    //    }
      
//    //    /// <summary>
//    //    /// 列关系数据查询
//    //    /// </summary>
//    //    /// <param name="tableInfo"></param>
//    //    /// <param name="matchColumns"></param>
//    //    /// <returns></returns>
//    //    private List<ColumnAttribute> GetColumnRelationData(TableInfo tableInfo, List<ColumnAttribute>  matchColumns)
//    //    {
           
//    //        #region 事先把需要从表中关联的数据查出来 
//    //        var relTableCols = matchColumns.Where(t => t.DataSourceType == 2);
//    //        foreach (var col in relTableCols)
//    //        {
//    //            StringBuilder sbSelect = new StringBuilder();
//    //            sbSelect.Append(string.Format(" select {0} as relKey,{1} as relValue  from {2}  ", col.MapKeyColName, col.MapValueColName, col.MapTableName));

//    //            if (col.MapValueColRelations != null && col.MapValueColRelations.Count > 0)
//    //            {
//    //                sbSelect.Append(" where ");
//    //            }
//    //            foreach (var r in col.MapValueColRelations)
//    //            {
//    //                switch (r.DataType)
//    //                {
//    //                    case "number":
//    //                        sbSelect.Append(string.Format(" {0}={1} and ", r.ColName, r.DataValue));
//    //                        break;
//    //                    default://"string"
//    //                        sbSelect.Append(string.Format(" {0}='{1}' and ", r.ColName, r.DataValue));
//    //                        break;
//    //                }
//    //            }
//    //            string sbRelColSql = sbSelect.ToString();
//    //            sbRelColSql = sbRelColSql.Substring(0, sbRelColSql.Length - 4);
//    //            DataTable dtRelCol = freeSql.Ado.ExecuteDataTable(sbRelColSql);
//    //            List<ColumnSourceSelectInfo> selectList = new List<ColumnSourceSelectInfo>();//给需要从表中查询的列赋值key ，value
//    //            for (int i = 0; i < dtRelCol.Rows.Count; i++)
//    //            {
//    //                ColumnSourceSelectInfo item = new ColumnSourceSelectInfo
//    //                {
//    //                    key = dtRelCol.Rows[i]["relKey"].ToString(),
//    //                    value = dtRelCol.Rows[i]["relValue"].ToString()
//    //                };
//    //                selectList.Add(item);
//    //            }
//    //            col.ColSelectList = selectList;
//    //        }
//    //        #endregion  
//    //        return matchColumns;
//    //    }

//    //    /// <summary>
//    //    /// 插入表的泪偷信息构造
//    //    /// </summary>
//    //    /// <param name="tableName"></param>
//    //    /// <param name="matchColumns"></param>
//    //    /// <param name="tableRelationColNames"></param>
//    //    /// <returns></returns>
//    //    private string CreateColumnNameSql(string tableName, List<ColumnAttribute> matchColumns, List<TableRelation> tableRelationColNames)
//    //    {
//    //        //sbInsertColumnsSql用于存列头信息   eg:    into sys_third_system_type(id, system_no, system_name, CREATEDATE,creater) 
//    //        StringBuilder sbInsertColumnsSql = new StringBuilder();
//    //        sbInsertColumnsSql.Append(string.Format(" into {0} (ID,", tableName));
//    //        foreach (var col in matchColumns)
//    //        {
//    //            sbInsertColumnsSql.Append(col.ColName + ",");
//    //        }

//    //        foreach (var tablRel in tableRelationColNames)
//    //        {
//    //            sbInsertColumnsSql.Append(tablRel.SecondaryColName + ",");  // 从表的字段名
//    //        }
//    //        sbInsertColumnsSql.Append("CREATEDATE,CREATER)");
//    //        return sbInsertColumnsSql.ToString();
//    //    }

//    //    /// <summary>
//    //    /// 校验行数据在数据表中是否已存在
//    //    /// </summary>
//    //    /// <param name="dr">行数据</param>
//    //    /// <param name="rowIndex">行号</param>
//    //    /// <param name="dt">数据表已有数据</param>
//    //    /// <param name="dataUniqueCol">校验必须唯一的列信息</param>
//    //    private void CheckDataBaseExist(DataRow dr,int rowIndex,DataTable dt,IEnumerable<ColumnAttribute> dataUniqueCol)
//    //    {
//    //        //校验数据是否在数据库中已经存在 
//    //        for (var q = 0; q < dt.Rows.Count; q++)
//    //        {
//    //            int matchCount = 0;
//    //            foreach (var unique in dataUniqueCol)
//    //            {
//    //                if (dr[unique.ExcelColName].ToString() == dt.Rows[q][unique.ColName].ToString())
//    //                {
//    //                    matchCount++;
//    //                }
//    //            }
//    //            if (dataUniqueCol != null && matchCount == dataUniqueCol.Count())
//    //            {
//    //                throw new BlocksBussnessException("101", L("第{0}行的数据在表中已存在", rowIndex + 1), null);
//    //            }
//    //        }
//    //    }

//    //    /// <summary>
//    //    /// 数据值校验
//    //    /// </summary>
//    //    /// <param name="dataValue">数据值</param>
//    //    /// <param name="rowIndex">行号</param>
//    //    /// <param name="col">列信息</param>
//    //    /// <returns>数据值（原值或转换成Key的值）</returns>
//    //    private string CheckDataValue(string dataValue, int rowIndex,ColumnAttribute col)
//    //    {                             
//    //        //长度校验
//    //        if (col.DataLength > 0 && col.DataType.Trim().ToLower().Equals("string") && dataValue.Length > col.DataLength)
//    //        {
//    //            throw new BlocksBussnessException("101", L("列{0}，第{1}行的数据值超长", col.ColName, rowIndex + 1), null);
//    //        }
//    //        //正则表达式校验 
//    //        if (!string.IsNullOrEmpty(col.DataValidRule) && !Regex.IsMatch(dataValue, col.DataValidRule))
//    //        {
//    //            throw new BlocksBussnessException("101", L("列{0}，第{1}行的数据值不符合要求：{2}", col.ColName, rowIndex + 1, col.DataValidRuleDesc), null);
//    //        }
//    //        //数据类型值切换，从value替换到key存储
//    //        if (col.DataSourceType == 1 || col.DataSourceType == 2)
//    //        {
//    //            var selInfo = col.ColSelectList.FirstOrDefault(t => t.value == dataValue);
//    //            if (selInfo == null)
//    //            {
//    //                throw new BlocksBussnessException("101", L("列{0}，第{1}行的数据值在XML配置文件中找到对应列转换值", col.ColName, rowIndex + 1), null);
//    //            }
//    //            dataValue = selInfo.key; 
//    //        }
//    //        return dataValue;
//    //    }
      
//    //    #endregion


//    //    #region 弃用代码
//    //    /// <summary>
//    //    /// 读取文件内容返回
//    //    /// </summary>
//    //    /// <param name="path"></param>
//    //    /// <returns></returns>
//    //    public static StringBuilder ReadFileContent(string path)
//    //    {
//    //        if (!File.Exists(path))
//    //        {
//    //            throw new BlocksException(L("指定目录：{0}下文件不存在", path));
//    //        }
//    //        StreamReader sr = new StreamReader(path, Encoding.Default);
//    //        StringBuilder content = new StringBuilder();
//    //        string lineContent = string.Empty;
//    //        while ((lineContent = sr.ReadLine()) != null)
//    //        {
//    //            content.Append(lineContent);
//    //        }
//    //        sr.Close();
//    //        return content;
//    //    }

//    //    public static Object GetDynamicClassBydt(DataTable dt)
//    //    {
//    //        dynamic d = new System.Dynamic.ExpandoObject();
//    //        //创建属性，并赋值。
//    //        foreach (DataColumn cl in dt.Columns)
//    //        {
//    //            (d as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(cl.ColumnName, dt.Rows[0][cl.ColumnName].ToString()));
//    //        }
//    //        return d;
//    //    }
//    //    #endregion
//    //}


//}
