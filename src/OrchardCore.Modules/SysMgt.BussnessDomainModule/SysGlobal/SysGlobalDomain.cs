//using Blocks.BussnessEntityModule;
//using BlocksCore.Data.Abstractions.Paging;
//using BlocksCore.Domain.Abstractions.Domain;
//using BlocksCore.Domain.Abstractions;
//using Microsoft.Extensions.Localization;
//using BlocksCore.Abstractions.Security;
//using SysMgt.BussnessDomainModule.Common;
//using SysMgt.BussnessDTOModule.Common;
//using SysMgt.BussnessDTOModule.SysGlobal;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Xml.Linq;

//namespace SysMgt.BussnessDomainModule.SysGlobal
//{  
//    public class SysGlobalDomain : IDomainService
//    {
//        public IUserContext UserContext { get; set; }
//        /// <summary>
//        /// 构造函数,实例化对象
//        /// </summary> 
//        public SysGlobalDomain(IUserContext userContext)
//        {
//            this.UserContext = userContext;
//        }

//        public SysGlobalData CustomerXml(SysGlobalData sysGlobalData)
//        {
//            string rtnHtml = "";
//            string tableName = "";
//            List<KeyValue> KeyValues = new List<KeyValue>();
//            List<KeyValue> DateList = new List<KeyValue>();
//            List<SelectValue> DDLList = new List<SelectValue>();
//            //通过KEY拿到对应类型文件存放路径，主信息ID
//            var pFilePath = @"C:\Users\ykzhu\Desktop\XML文件配置预设.xml";
//            //xmlstring与路径进行拼接
//            //读取XML
//            //按逻辑解析XML得到字段及其配置形成List
//            //由表名与主信息ID进行查询得到每个属性值

//            XDocument xmlDoc = XDocument.Load(pFilePath);
//            XElement root = xmlDoc.Root;
//            //根节点
//            if (!string.Equals(root.Name.ToString(), "customerTemplate"))
//            {
//                throw new BlocksBussnessException(L["未读取到配置文件中的<customerTemplate>节点"]);
//            }
//            //主表节点 获取表名
//            XElement customerTable = root.Element("customerTable");
//            if (customerTable == null)
//            {
//                throw new BlocksBussnessException(L["未读取到配置文件中的<customerTable>节点"]);
//            }
//            if (customerTable.Attribute("tableName") == null || string.IsNullOrEmpty(customerTable.Attribute("tableName").Value)) //属性中配置表名
//            {
//                throw new BlocksBussnessException(L["配置文件中<customerTable>节点属性tableName缺失或值为空"]);
//            }
//            tableName = customerTable.Attribute("tableName").Value;
//            //解析表中的列
//            if (customerTable.Elements("column").Count() == 0)
//            {
//                throw new BlocksBussnessException(L["配置文件中<customerTable>节点下未配置<column>节点信息"]);
//            }
//            foreach (XElement col in customerTable.Elements())
//            {
//                #region 参数定义
//                string codeName = "";
//                string label = "";
//                string dataType = "";
//                string displaySize = "";
//                string displayRequired = "";
//                string displayMin = "";
//                string displayMax = "";
//                string displayReadonly = "";
//                string displayHidden = "";
//                string displayType = "";
//                string dateformat = "";
//                string inputType = "";
//                string displayName = "";
//                string displayValue = "";
//                string selectType = "";
//                string selectUrl = "";
//                string selectMultiple = "";
//                string selectAllowClear = "";
//                #endregion
//                //解析字段基础属性
//                if (!string.Equals(col.Name.ToString(), "column")) { continue; }
//                if (col.Attribute("colName") == null || string.IsNullOrEmpty(col.Attribute("colName").Value))
//                {
//                    throw new BlocksBussnessException(L["配置文件<customerTable>节点下子节点<column>节点中缺失colName属性或值为空"]);
//                }
//                if (col.Attribute("dataType") == null || string.IsNullOrEmpty(col.Attribute("dataType").Value))
//                {
//                    throw new BlocksBussnessException(L["配置文件<customerTable>节点下子节点<column>节点中缺失dataType属性或值为空"]);
//                }
//                if (col.Attribute("label") == null || string.IsNullOrEmpty(col.Attribute("label").Value))
//                {
//                    throw new BlocksBussnessException(L["配置文件<customerTable>节点下子节点<column>节点中缺失label属性或值为空"]);
//                }
//                codeName = col.Attribute("colName").Value;
//                dataType = col.Attribute("dataType").Value;
//                label = col.Attribute("label").Value;

//                KeyValue KeyValue = new KeyValue();
//                KeyValue.Key = codeName;
//                KeyValues.Add(KeyValue);

//                //解析字段显示属性
//                XElement colDisplay = col.Element("display");
//                if (colDisplay == null)
//                {
//                    throw new BlocksBussnessException(L["未读取到配置文件中的<display>节点"]);
//                }
//                if (colDisplay.Attribute("type") == null || string.IsNullOrEmpty(colDisplay.Attribute("type").Value))
//                {
//                    throw new BlocksBussnessException(L["配置文件<column>节点下子节点<display>节点中缺失type属性或值为空"]);
//                }
//                displayType = colDisplay.Attribute("type").Value;
//                if (colDisplay.Attribute("size") == null || string.IsNullOrEmpty(colDisplay.Attribute("size").Value))
//                {
//                    displaySize = "6";
//                }
//                else
//                {
//                    displaySize = colDisplay.Attribute("size").Value;
//                }
//                if (colDisplay.Attribute("readonly") == null || string.IsNullOrEmpty(colDisplay.Attribute("readonly").Value))
//                {
//                    displayReadonly = "";
//                }
//                else
//                {
//                    displayReadonly = colDisplay.Attribute("readonly").Value;
//                }
//                if (colDisplay.Attribute("hidden") == null || string.IsNullOrEmpty(colDisplay.Attribute("hidden").Value))
//                {
//                    displayHidden = "";
//                }
//                else
//                {
//                    displayHidden = colDisplay.Attribute("hidden").Value;
//                }

//                //根据显示标签中type属性决定读取下层哪种标签
//                switch (displayType)
//                {
//                    case "input":
//                        XElement colInput = colDisplay.Element("input");
//                        if (colInput == null)
//                        {
//                            throw new BlocksBussnessException(L["未读取到配置文件中的<input>节点"]);
//                        }
//                        if (colInput.Attribute("type") == null || string.IsNullOrEmpty(colInput.Attribute("type").Value))
//                        {
//                            inputType = "text";
//                        }
//                        else
//                        {
//                            inputType = colInput.Attribute("type").Value;
//                        }
//                        if (colInput.Attribute("name") == null || string.IsNullOrEmpty(colInput.Attribute("name").Value))
//                        {
//                            displayName = codeName;
//                        }
//                        else
//                        {
//                            displayName = colInput.Attribute("name").Value;
//                        }
//                        if (colInput.Attribute("value") == null || string.IsNullOrEmpty(colInput.Attribute("value").Value))
//                        {
//                            if (inputType == "radio" || inputType == "checkbox")
//                            {
//                                throw new BlocksBussnessException(L["配置文件<display>节点下子节点<input>节点中缺失value属性或值为空"]);
//                            }
//                            displayValue = "";
//                        }
//                        else
//                        {
//                            displayValue = colInput.Attribute("value").Value;
//                        }
//                        if (colInput.Attribute("dateformat") == null || string.IsNullOrEmpty(colInput.Attribute("dateformat").Value))
//                        {
//                            dateformat = "";
//                        }
//                        else
//                        {
//                            dateformat = colInput.Attribute("dateformat").Value;
//                            if (dataType != "datetime" || inputType != "text")
//                            {
//                                throw new BlocksBussnessException(L["配置文件<display>节点下子节点<input>节点中dateformat属性配置与其数据类型、控件类型不相符"]);
//                            }
//                            KeyValue DateValue = new KeyValue();
//                            DateValue.Key = codeName;
//                            DateValue.Value = dateformat;
//                            DateList.Add(DateValue);
//                        }
//                        break;
//                    case "select":
//                        XElement colSelect = colDisplay.Element("select");
//                        if (colSelect == null)
//                        {
//                            throw new BlocksBussnessException(L["未读取到配置文件中的<select>节点"]);
//                        }
//                        if (dataType == "datetime")
//                        {
//                            throw new BlocksBussnessException(L["配置文件<display>节点中type属性配置与其数据类型不相符"]);
//                        }
//                        //
//                        if (colSelect.Attribute("dataType") == null || string.IsNullOrEmpty(colSelect.Attribute("dataType").Value))
//                        {
//                            selectType = "";
//                        }
//                        else
//                        {
//                            selectType = colSelect.Attribute("dataType").Value;
//                            if (colSelect.Attribute("multiple") == null || string.IsNullOrEmpty(colSelect.Attribute("multiple").Value))
//                            {
//                                selectMultiple = "0";
//                            }
//                            else
//                            {
//                                selectMultiple = "1";
//                            }
//                            if (colSelect.Attribute("allowClear") == null || string.IsNullOrEmpty(colSelect.Attribute("allowClear").Value))
//                            {
//                                selectAllowClear = "0";
//                            }
//                            else
//                            {
//                                selectAllowClear = "1";
//                            }
//                            SelectValue SelectValue = new SelectValue();
//                            List<KeyValue> SelectList = new List<KeyValue>();
//                            SelectValue.Key = codeName;
//                            SelectValue.Type = selectType;
//                            SelectValue.Multiple = selectMultiple;
//                            SelectValue.AllowClear = selectAllowClear;                           
//                            switch (selectType)
//                            {
//                                case "list":
//                                    //拿到下面li标签
//                                    if (colSelect.Elements("li").Count() == 0)
//                                    {
//                                        throw new BlocksBussnessException(L["配置文件中<select>节点下未配置<li>节点信息"]);
//                                    }
//                                    foreach (XElement select in colSelect.Elements())
//                                    {
//                                        if (!string.Equals(select.Name.ToString(), "li")) { continue; }
//                                        if (select.Attribute("key") == null || string.IsNullOrEmpty(select.Attribute("key").Value))
//                                        {
//                                            throw new BlocksBussnessException(L["配置文件<select>节点下子节点<li>节点中缺失key属性或值为空"]);
//                                        }
//                                        if (select.Value == null || string.IsNullOrEmpty(select.Value))
//                                        {
//                                            throw new BlocksBussnessException(L["配置文件<select>节点下子节点<li>节点中缺失值或值为空"]);
//                                        }
//                                        KeyValue selectData = new KeyValue();
//                                        selectData.Key = select.Attribute("key").Value;
//                                        selectData.Value = select.Value;
//                                        SelectList.Add(selectData);
//                                    }
//                                        SelectValue.SelectList = SelectList;
//                                    break;
//                                case "api":
//                                    if (colSelect.Attribute("url") == null || string.IsNullOrEmpty(colSelect.Attribute("url").Value))
//                                    {
//                                        throw new BlocksBussnessException(L["配置文件<select>节点中url属性配置有误"]);
//                                    }
//                                    else
//                                    {
//                                        selectUrl = displayValue = colSelect.Attribute("url").Value;
//                                        SelectValue.Url = selectUrl;
//                                    }
//                                    break;
//                                default:
//                                    throw new BlocksBussnessException(L["配置文件<display>节点中type属性配置错误"]);
//                            }
//                            DDLList.Add(SelectValue);
//                        }
//                        break;
//                    default:
//                        throw new BlocksBussnessException(L["配置文件<column>节点下子节点<display>节点中type属性配置错误！"]);
//                }

//                //解析列中验证部分标签
//                XElement colCheck = col.Element("check");
//                if (colCheck == null)
//                {
//                    throw new BlocksBussnessException(L["未读取到配置文件中的<check>节点"]);
//                }
//                if (colCheck.Attribute("min") == null || string.IsNullOrEmpty(colCheck.Attribute("min").Value))
//                {
//                    displayMin = "";
//                }
//                else
//                {
//                    displayMin = colCheck.Attribute("min").Value;
//                    int min;
//                    bool isnumber = int.TryParse(displayMin,out min);
//                    if (!isnumber)
//                    {
//                        throw new BlocksBussnessException(L["配置文件<column>节点下子节点<display>节点中min属性未正确填写正整数！"]);
//                    }
//                }
//                if (colCheck.Attribute("max") == null || string.IsNullOrEmpty(colCheck.Attribute("max").Value))
//                {
//                    displayMax = "";
//                }
//                else
//                {
//                    displayMax = colCheck.Attribute("max").Value;
//                    int max;
//                    bool isnumber = int.TryParse(displayMax, out max);
//                    if (!isnumber)
//                    {
//                        throw new BlocksBussnessException(L["配置文件<column>节点下子节点<display>节点中max属性未正确填写正整数！"]);
//                    }
//                }
//                if (colCheck.Attribute("required") == null || string.IsNullOrEmpty(colCheck.Attribute("required").Value))
//                {
//                    displayRequired = "";
//                }
//                else
//                {
//                    displayRequired = colCheck.Attribute("required").Value;
//                }

//                //当前列解析结束开始拼接html字符串
//                string colHtml = "";
//                switch (displayType)
//                {
//                    case "input":
//                        //字段占位1 - 12 是否隐藏
//                        colHtml += "<div class='col-sm-" + displaySize + "' "; 
//                        if (displayHidden == "1")
//                        {
//                            colHtml += "style = 'display:none' ";
//                        }
//                        colHtml += ">";
//                        //固定样式加载
//                        colHtml += "<div class='form-group form-float'> <div class='form-line'>";
//                        //字段属性 必填
//                        colHtml += "<input class='form-control' type ='" + inputType + "' id='" + codeName + "' name ='" + displayName + "' ";
//                        //字段属性 可选
//                        if (displayMax != "")
//                        {
//                            colHtml += "maxlength ='" + displayMax + "' ";
//                        }
//                        if (displayMin != "")
//                        {
//                            colHtml += "minlength ='" + displayMin + "' ";
//                        }
//                        if (displayReadonly == "1")
//                        {
//                            colHtml += "readonly ='readonly' ";
//                        }
//                        colHtml += ">";
//                        //标题属性
//                        if (displayRequired == "1")
//                        {
//                            colHtml += "<label class='form-label required'> " + label + "：</label>";
//                        }
//                        else
//                        {
//                            colHtml += "<label class='form-label'> " + label + "：</label>";
//                        }
//                        //收尾
//                        colHtml += "</div></div></div>";
//                        break;
//                    case "select":
//                        //字段占位1 - 12 是否隐藏
//                        colHtml += "<div class='col-sm-" + displaySize + "' ";
//                        if (displayHidden == "1")
//                        {
//                            colHtml += "style = 'display:none' ";
//                        }
//                        colHtml += ">";
//                        //固定样式加载
//                        colHtml += "<div class='form-group form-float'>";
//                        //标题属性
//                        if (displayRequired == "1")
//                        {
//                            colHtml += "<label class='form-label required'> " + label + "：</label>";
//                        }
//                        else
//                        {
//                            colHtml += "<label class='form-label'> " + label + "：</label>";
//                        }
//                        //字段属性 必填
//                        colHtml += "<select class='form-control' v-select2='' id='" + codeName + "' name ='" + codeName + "' ";
//                        if (displayReadonly == "1")
//                        {
//                            colHtml += "readonly ='readonly' ";
//                        }
//                        colHtml += "></select>";
//                        //收尾
//                        colHtml += "</div></div>";
//                        break;
//                    default:
//                        break;
//                }
//                rtnHtml += colHtml;

//            }
//            //验证XML文件colName字段是否有重复值

//            //通过code列表拼SQL拿到数据List,并且对返回值进行赋值
//            if (sysGlobalData.Cid != null && sysGlobalData.Cid!="")
//            {
//                //拿到查询数据
//                StringBuilder sb = new StringBuilder();
//                sb.Append("select * from " + tableName + " where M_ID ='" + sysGlobalData.Cid + "' ");
//                string strSQL = sb.ToString();
//                //执行SQL语句
//                IFreeSql freeSql = SqlConnect.Instance;
//                var dataList = freeSql.Ado.ExecuteDataTable(strSQL);
//                if (dataList.Rows.Count > 0)
//                {
//                    foreach (var keyValue in KeyValues)
//                    {
//                        var dateC = DateList.FirstOrDefault(t => t.Key == keyValue.Key);
//                        if (dateC == null)
//                        {
//                            keyValue.Value = dataList.Rows[0][keyValue.Key].ToString();
//                        }
//                        else
//                        {
//                            DateTime dt;
//                            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
//                            dtFormat.ShortDatePattern = dateC.Value;
//                            dt = Convert.ToDateTime(dataList.Rows[0][keyValue.Key].ToString(), dtFormat);
//                            keyValue.Value = dt.ToString(dateC.Value);
//                        }
//                    }
//                }
//            }
//            SysGlobalData rtnData = new SysGlobalData();
//            rtnData.HtmlString = rtnHtml;
//            rtnData.ValueList = KeyValues;
//            rtnData.DateList = DateList;
//            rtnData.DDLList = DDLList;
//            return rtnData;
//        }

//        public string OperateCustomerSQL(SysGlobalData sysGlobalData)
//        {
//            //由sysGlobalData.CKey得到XM路径，解析XML得到表名及字段数据库属性
//            string tableName = "";
//            List<KeyValue> KeyValues = new List<KeyValue>();
//            //通过KEY拿到对应类型文件存放路径，主信息ID
//            var pFilePath = @"C:\Users\ykzhu\Desktop\XML文件配置预设.xml";

//            XDocument xmlDoc = XDocument.Load(pFilePath);
//            XElement root = xmlDoc.Root;
//            //根节点
//            if (!string.Equals(root.Name.ToString(), "customerTemplate"))
//            {
//                throw new BlocksBussnessException(L["未读取到配置文件中的<customerTemplate>节点"]);
//            }
//            //主表节点 获取表名
//            XElement customerTable = root.Element("customerTable");
//            if (customerTable == null)
//            {
//                throw new BlocksBussnessException(L["未读取到配置文件中的<customerTable>节点"]);
//            }
//            if (customerTable.Attribute("tableName") == null || string.IsNullOrEmpty(customerTable.Attribute("tableName").Value)) //属性中配置表名
//            {
//                throw new BlocksBussnessException(L["配置文件中<customerTable>节点属性tableName缺失或值为空"]);
//            }
//            tableName = customerTable.Attribute("tableName").Value;
//            //解析表中的列
//            if (customerTable.Elements("column").Count() == 0)
//            {
//                throw new BlocksBussnessException(L["配置文件中<customerTable>节点下未配置<column>节点信息"]);
//            }
//            foreach (XElement col in customerTable.Elements())
//            {
//                #region 参数定义
//                string codeName = "";
//                string dataType = "";
//                #endregion
//                //解析字段基础属性
//                if (!string.Equals(col.Name.ToString(), "column")) { continue; }
//                if (col.Attribute("colName") == null || string.IsNullOrEmpty(col.Attribute("colName").Value))
//                {
//                    throw new BlocksBussnessException(L["配置文件<customerTable>节点下子节点<column>节点中缺失colName属性或值为空"]);
//                }
//                if (col.Attribute("dataType") == null || string.IsNullOrEmpty(col.Attribute("dataType").Value))
//                {
//                    throw new BlocksBussnessException(L["配置文件<customerTable>节点下子节点<column>节点中缺失dataType属性或值为空"]);
//                }
//                codeName = col.Attribute("colName").Value;
//                dataType = col.Attribute("dataType").Value;

//                KeyValue KeyValue = new KeyValue();
//                KeyValue.Key = codeName;
//                KeyValue.Value = dataType;
//                KeyValues.Add(KeyValue);
//            }

//            StringBuilder sb = new StringBuilder();
//            switch (sysGlobalData.OperateType)
//            {
//                case "INSERT":
//                    //用于存储返回的整体 sql
//                    sb.Append(" insert ");

//                    //用于存列信息 eg: into sys_third_system_type(id, system_no, system_name, CREATEDATE,creater) 
//                    StringBuilder sbInsertColumnsSql = new StringBuilder();
//                    sbInsertColumnsSql.Append(string.Format(" into {0} (ID,M_ID,", tableName));
//                    //用来构造一条数据sql 
//                    StringBuilder sbDataSql = new StringBuilder();
//                    sbDataSql.Append(" values ('" + Guid.NewGuid().ToString() + "','" + sysGlobalData.Cid + "',");//Id,主信息Id

//                    foreach (var col in sysGlobalData.ValueList)
//                    {
//                        if (col.Value == null || col.Value == "")
//                        {
//                            continue;
//                        }
//                        sbInsertColumnsSql.Append(col.Key + ",");
//                        var data = KeyValues.Find(t => t.Key == col.Key);
//                        switch (data.Value)
//                        {
//                            case "string":
//                                sbDataSql.Append("'" + col.Value.ToString() + "',");
//                                break;
//                            case "datetime":
//                                sbDataSql.Append(string.Format("to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'),", col.Value.ToString()));
//                                break;
//                            case "number":
//                                sbDataSql.Append(string.Format("to_number('{0}'),", col.Value.ToString()));
//                                break;
//                            default:
//                                //错误 
//                                break;
//                        }
//                    }
//                    //拼上默认两列
//                    sbInsertColumnsSql.Append("CREATEDATE,CREATER)");
//                    sbDataSql.Append(string.Format("to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'),'{1}')", DateTime.Now.ToString(), UserContext.GetCurrentUser().UserId));//to_date('2019-07-01 19:56:54','YYYY-MM-DD HH24:MI:SS')
//                    string strInsertColumnsSql = sbInsertColumnsSql.ToString();
//                    string strDataSql = sbDataSql.ToString();

//                    sb.Append(strInsertColumnsSql);
//                    sb.Append(strDataSql);
//                    //sb.Append(" select 1 from dual ");
//                    break;
//                case "UPDATE":                    
//                    //用于存储返回的整体 sql
//                    sb.Append("update "+ tableName + " set ");
//                    foreach (var col in sysGlobalData.ValueList)
//                    {
//                        sb.Append(col.Key + "=");
//                        var data = KeyValues.Find(t => t.Key == col.Key);
//                        switch (data.Value)
//                        {
//                            case "string":
//                                sb.Append("'" + col.Value.ToString() + "',");
//                                break;
//                            case "datatime":
//                                sb.Append(string.Format("to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'),", col.Value.ToString()));
//                                break;
//                            case "number":
//                                sb.Append(string.Format("to_number('{0}'),", col.Value.ToString()));
//                                break;
//                            default:
//                                //错误 
//                                break;
//                        }
//                    }
//                    sb.Append(string.Format("UPDATEDATE = to_date('{0}', 'YYYY-MM-DD HH24:MI:SS'), UPDATER = '{1}'", DateTime.Now.ToString(), UserContext.GetCurrentUser().UserId));
//                    sb.Append("where M_ID ='" + sysGlobalData.Cid + "'");
//                    break;
//                case "DELETE":
//                    sb.Append("delete from " + tableName + " where M_ID ='" + sysGlobalData.Cid + "' ");
//                    break;
//                default:
//                    break;
//            }
//            string strSQL = sb.ToString();
//            //执行SQL语句
//            IFreeSql freeSql = SqlConnect.Instance;
//            int successCount = freeSql.Ado.ExecuteNonQuery(strSQL);
//            if (successCount <= 0)
//            {
//                throw new BlocksBussnessException("101", L["导入失败"], null);
//            }
//            return "";
//        }
//    }
//}
