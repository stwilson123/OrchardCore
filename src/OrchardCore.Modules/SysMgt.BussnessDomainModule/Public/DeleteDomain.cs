
//using BlocksCore.Domain.Abstractions.Domain;
//using BlocksCore.Domain.Abstractions;
//using BlocksCore.Localization.Abtractions;

//using System.Collections.Generic;
//using System.Linq;
//using System.IO;
//using System.Xml.Linq;

//using SysMgt.BussnessRespositoryModule.Public;
//using SysMgt.BussnessDTOModule.Public;
//using System.Web;
//using System.Text;

//namespace SysMgt.BussnessDomainModule.Public
//{
//    public class DeleteDomain : IDomainService
//    {
//        public Localizer L { get; set; }

//        public IDeleteRepository DeleteRepository { get; set; }

//        public DeleteDomain(IDeleteRepository deleterepository)
//        {

//            this.DeleteRepository = deleterepository;

//        }

//        public string Delete(DeleteInfo deleteInfo)
//        {

//            #region 通用判断

//            string id = deleteInfo.ID;

//            List<string> ids = deleteInfo.IDS;

//            string xmlName = deleteInfo.XmlPatch;

//            if (string.IsNullOrEmpty(id) && (ids == null || ids.Count == 0))
//            {
//                throw new BlocksBussnessException("101", L("请选择需要删除的数据!"), null);
//            }

//            if (string.IsNullOrEmpty(xmlName))
//            {
//                throw new BlocksBussnessException("101", L("没有获取到配置文件!"), null);
//            }

//            string xmlPath = HttpContext.Current.Server.MapPath("~/xml/" + xmlName + ".xml");//这边需要改造，怎么能跨机器拿到这个xml文件呢？

//            if (!File.Exists(xmlPath))
//            {
//                throw new BlocksBussnessException("101", L("配置文件不存在!"), null);
//            }

//            #endregion

//            if (!string.IsNullOrEmpty(deleteInfo.ID))
//            {
//                deleteInfo.IDS = new List<string>();
//                deleteInfo.IDS.Add(deleteInfo.ID);
//            }

//            DeleteList(deleteInfo.IDS, xmlPath);


//            return L("succeed");

//        }

//        private void DeleteList(List<string> idlist, string xmlpath)
//        {

//            #region 判断是否可以删除
//            string ServerQueryString = "";
//            XDocument document = XDocument.Load(xmlpath);
//            XElement root = document.Root;

//            #region 获取主表
//            string mtTable = string.Empty;//数据表名称
//            string mtField = string.Empty;//数据表字段
//            string tipCol = string.Empty;//提示的数据表字段
//            IEnumerable<XElement> masterTable = root.Elements("masterTable").Elements();
//            foreach (XElement item in masterTable)
//            {
//                mtTable = item.Attribute("name").Value.Trim();//数据表名称
//                mtField = item.Attribute("field").Value.Trim();//数据表字段
//                tipCol = item.Attribute("tipField").Value.Trim();//提示的数据表字段
//            }                                           //string businessname = masteritem.Attribute("businessname").Value.Trim();
//            #endregion

//            #region 把List中的字符串变成逗号分割，可以用于in中的查询
//            string ids = string.Empty;
//            for (int i = 0; i < idlist.Count; i++)
//            {
//                ids += "'" + idlist[i] + "'";
//                ids += " ,";
//            }
//            ids = ids.TrimEnd(',');
//            #endregion


//            IEnumerable<XElement> serviceTable = root.Elements("serviceTable").Elements();
//            foreach (XElement item in serviceTable)
//            {

//                string tabelname = item.Attribute("name").Value.Trim();
//                string colname = item.Attribute("field").Value.Trim();
//                string tip = item.Attribute("tip").Value.Trim();

//                //ServerQueryString += " select DISTINCT '" + tip + "' Tip, b.id ID ,b." + tipcol + " Code  from  " + tabelname + " a inner join  " + mktable + " b on a." + colname + "=b.id  where  a." + colname + "    in (" + ids + ")";
//                ServerQueryString += " select DISTINCT '" + tip + "' Tip, a." + mtField + " ID ,a." + tipCol + " Code  from  " + mtTable + " a inner join  " + tabelname + " b on a." + mtField + "=b." + colname + "  where  a." + mtField + "    in (" + ids + ")";
//                ServerQueryString += " union all ";
//            }

//            #region 处理需要删除前验证的逻辑
//            if (serviceTable.Count() > 0)
//            {
//                ServerQueryString = ServerQueryString.Trim().Substring(0, ServerQueryString.Length - 11);//去掉末尾的union all
//                var notlist = DeleteRepository.GetNotDeleteList(ServerQueryString);//执行sql语句
//                                                                                   // notlist = notlist.Distinct().ToList();

//                StringBuilder tipInfo = new StringBuilder();
//                foreach (var item in notlist)
//                {
//                    tipInfo.Append(item.Tip.Replace("[0]", item.Code) + ";");//怎么翻译？
//                }

//                if (tipInfo.Length > 0)
//                {
//                    throw new BlocksBussnessException("101", L(tipInfo.ToString().TrimEnd(';')), null);
//                }
//            }
//            #endregion

//            #endregion

//            #region 执行删除操作         
//            IEnumerable<XElement> deleteTabel = root.Elements("deleteTabel").Elements();
//            foreach (XElement item in deleteTabel)
//            {
//                string tabelname = item.Attribute("name").Value.Trim();
//                string colname = item.Attribute("field").Value.Trim();
//                string deleteSQLString = " delete from " + tabelname + " where  " + colname + "    in (" + ids + ")";
//                DeleteRepository.ExeDelete(deleteSQLString);
//            }
//            #endregion

//        }

//    }
//}
