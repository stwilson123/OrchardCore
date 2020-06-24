using BlocksCore.Domain.Abstractions.Domain;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using SysMgt.BussnessDomainModule.Common;
using SysMgt.BussnessDTOModule.Setup;
using SysMgt.BussnessRespositoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions.UI.Combobox;
using BlocksCore.Domain.Abstractions;
using BlocksCore.Localization.Abtractions;
using SysMgt.BussnessDomainModule.ProductElement;
using SysMgt.BussnessDTOModule.Combobox;
using SysMgt.BussnessDTOModule.ProductFormat;
using Microsoft.Extensions.Logging;

namespace SysMgt.BussnessDomainModule.ProductFormat
{
    public class ProductFormatDomain : IDomainService
    {   
        public IProductFormatRepository ProductFormatRepository { get; set; }
        public IProductFormatDetailRepository ProductFormatDetailRepository { get; set; }
        public IProductElementRepository ProductElementRepository { get; set; }
        public ISetupRepository SetupRepository { get; set; }
        public IProduct4ElementRuleRepository Product4ElementRuleRepository { get; set; }
        public IProduct4VarElementRepository Product4VarElementRepository { get; set; }
        private IBdtaQueueRepository BdtaQueueRepository { get; set; }
        private ILogger Ilog { get; set; }
        public Localizer L { get; set; }
        public ProductFormatDomain(ILogger<ProductFormatDomain> Ilog,IProductFormatRepository productFormatRepository, IProductFormatDetailRepository productFormatDetailRepository, IBdtaQueueRepository BdtaQueueRepository, ISetupRepository setupRepository, IProductElementRepository productElementRepository, IProduct4ElementRuleRepository product4ElementRuleRepository, IProduct4VarElementRepository product4VarElementRepository)
        {
            this.ProductFormatRepository = productFormatRepository;
            this.ProductFormatDetailRepository = productFormatDetailRepository;
            this.BdtaQueueRepository = BdtaQueueRepository;
            this.SetupRepository = setupRepository;
            this.ProductElementRepository = productElementRepository;
            this.Product4ElementRuleRepository = product4ElementRuleRepository;
            this.Product4VarElementRepository = product4VarElementRepository;
            this.Ilog = Ilog;
        }

        public string Add(ProductFormatData productFormatData)
        {
            var productFormat = ProductFormatRepository.FirstOrDefault(t => t.PRODUCTFORMAT_CODE == productFormatData.Code);
            if (productFormat != null)
            {

                throw new BlocksBussnessException("101", L("编码不能相同!"), null);
            }

            if (productFormatData.ProductElements == null || !productFormatData.ProductElements.Any())
            {
                throw new BlocksBussnessException("101", L("未包含任何编码元素!"), null);
               
            }

            BDTA_PRODUCTFORMAT bdtaProductformat = new BDTA_PRODUCTFORMAT();
            bdtaProductformat.Id = Guid.NewGuid().ToString();
            //if (productFormatData.Code == "")
            //{
            //    throw new BlocksBussnessException("101", L("编码不能为空!"), null);
            //}
            bdtaProductformat.PRODUCTFORMAT_CODE = productFormatData.Code;
            //if (productFormatData.Name == "")
            //{
            //    throw new BlocksBussnessException("101", L("名称不能为空!"), null);
            //}
            bdtaProductformat.PRODUCTFORMAT_NAME = productFormatData.Name;
            bdtaProductformat.PRODUCTFORMAT_DESCRIPTION = productFormatData.Description;

            List<BDTA_PRODUCTFORMAT_DETAIL> productformatDetails = new List<BDTA_PRODUCTFORMAT_DETAIL>();
          
            long beginIndex = 0;
            long seq = 0;
            foreach (var item in productFormatData.ProductElements)
            {
                if (item.ProductElementName == "")
                {
                    throw new BlocksBussnessException("101", L("明细名称必选!"), null);
                }
                if (item.ProductformatStart == "")
                {
                    throw new BlocksBussnessException("101", L("起始位必填!"), null);
                }
                if (item.ProductformatEnd == "")
                {
                    throw new BlocksBussnessException("101", L("终止位必填!"), null);
                }
                BDTA_PRODUCTFORMAT_DETAIL bdtaProductformatDetail = new BDTA_PRODUCTFORMAT_DETAIL()
                {
                    Id = Guid.NewGuid().ToString(),
                    PRODUCTELEMENTID = item.ProductElementID,
                    PRODUCTFORMATID = bdtaProductformat.Id,
                    PRODUCTFORMAT_DETAIL_SEQ = seq++,
                    PRODUCTFORMAT_DETAIL_NAME = item.ProductElementName,
                    PRODUCTFORMAT_DETAIL_BEGIN = beginIndex + 1,
                    PRODUCTFORMAT_DETAIL_END = beginIndex + int.Parse(item.ProductElementLength),
                    PRODUCTFORMAT_DETAIL_LENTH = int.Parse(item.ProductElementLength),
                    PRODUCTFORMAT_START=item.ProductformatStart,
                    PRODUCTFORMAT_END=item.ProductformatEnd
                };
                beginIndex = beginIndex + int.Parse(item.ProductElementLength);
                productformatDetails.Add(bdtaProductformatDetail);
            }
            bdtaProductformat.PRODUCTFORMAT_LENGTH = beginIndex;
            ProductFormatRepository.InsertAndGetId(bdtaProductformat);
            ProductFormatDetailRepository.Insert(productformatDetails);
            return "新增成功";
        }

        public virtual string Delete(ProductFormatData productFormatData)
        {

            for (var I = 0; I < productFormatData.IDS.Count; I++)
            {
                var id = productFormatData.IDS[I];
                var exsitData = ProductFormatRepository.FirstOrDefault(t => t.Id == id);
                if (exsitData == null)
                {
                    throw new BlocksBussnessException("101", L("未查到对象"), null);
                }
                ProductFormatRepository.Delete(t => t.Id == id);
                ProductFormatDetailRepository.Delete(t => t.PRODUCTFORMATID == id);
            }
           
            //var exsitDataDetail = ProductFormatDetailRepository.FirstOrDefault(t => t.PRODUCTFORMATID == productFormatData.ID);
            //if (exsitDataDetail != null)
            //{
            //    throw new BlocksBussnessException("101", L("已绑定明细，请先删除明细再执行此操作!"), null);
            //}

            

            return "删除成功！";

        }
        public string Update(ProductFormatData productFormatData)
        {
            var productFormat = ProductFormatRepository.FirstOrDefault(t => t.Id == productFormatData.ID);
            if (productFormat == null)
            {
                throw new BlocksBussnessException("101", L("未查询到任何元素!"), null);
              
            }

            //if (productFormat.PRODUCTFORMAT_CODE == productFormatData.Code)
            //{
            //    throw new BlocksBussnessException("101", L("编码不能相同!"), null);
               
            //}

            if (productFormatData.ProductElements == null || !productFormatData.ProductElements.Any())
            {
                throw new BlocksBussnessException("101", L("未包含任何编码元素!"), null);
               
            }

            List<BDTA_PRODUCTFORMAT_DETAIL> productformatDetails = new List<BDTA_PRODUCTFORMAT_DETAIL>();

            long beginIndex = 0;
            long seq = 0;
            foreach (var item in productFormatData.ProductElements)
            {
                for (int i = 0; i < productFormatData.ProductElements.Count; i++)
                {
                    if (productFormatData.ProductElements[i].ProductElementName == "")
                    {
                        throw new BlocksBussnessException("101", L("明细名称必选!"), null);
                    }
                    if (productFormatData.ProductElements[i].ProductformatStart == "")
                    {
                        throw new BlocksBussnessException("101", L("起始位必填!"), null);
                    }
                    if (productFormatData.ProductElements[i].ProductformatEnd == "")
                    {
                        throw new BlocksBussnessException("101", L("终止位必填!"), null);
                    }
                }
                BDTA_PRODUCTFORMAT_DETAIL bdtaProductformatDetail = new BDTA_PRODUCTFORMAT_DETAIL()
                {
                    Id = Guid.NewGuid().ToString(),
                    PRODUCTELEMENTID = item.ProductElementID,
                    PRODUCTFORMATID = productFormat.Id,
                    PRODUCTFORMAT_DETAIL_SEQ = seq++,
                    PRODUCTFORMAT_DETAIL_NAME = item.ProductElementName,
                    PRODUCTFORMAT_DETAIL_BEGIN = beginIndex + 1,
                    PRODUCTFORMAT_DETAIL_END = beginIndex + int.Parse(item.ProductElementLength),
                    PRODUCTFORMAT_DETAIL_LENTH = int.Parse(item.ProductElementLength),
                    PRODUCTFORMAT_START = item.ProductformatStart,
                    PRODUCTFORMAT_END = item.ProductformatEnd
                };
                beginIndex = beginIndex + int.Parse(item.ProductElementLength);
                productformatDetails.Add(bdtaProductformatDetail);
            }

            ProductFormatDetailRepository.Delete(t => t.PRODUCTFORMATID == productFormat.Id);
            ProductFormatRepository.Update(t => t.Id == productFormatData.ID, t => new BDTA_PRODUCTFORMAT()
            {
                PRODUCTFORMAT_CODE = productFormatData.Code,
                PRODUCTFORMAT_NAME = productFormatData.Name,
                PRODUCTFORMAT_DESCRIPTION = productFormatData.Description,
                PRODUCTFORMAT_LENGTH = beginIndex
            });
            ProductFormatDetailRepository.Insert(productformatDetails);
            return "保存成功";
        }

        public ProductFormatData GetOneById(ProductFormatData productFormatData)
        {
            var productFormat = ProductFormatRepository.FirstOrDefault(t => t.Id == productFormatData.ID);
            if (productFormat == null)
            {
                throw new BlocksBussnessException("101", L("未查到对象"), null);
            }

            var productFormatDetail = ProductFormatDetailRepository.GetAllList(t=>t.PRODUCTFORMATID==productFormatData.ID).OrderBy(t=>t.PRODUCTFORMAT_DETAIL_SEQ);
            List<ProductElementInfo> list=new List<ProductElementInfo>();

            if (productFormatDetail != null)
            {
                foreach (var item in productFormatDetail)
                {
                    list.Add(new ProductElementInfo()
                    {
                        ID = item.Id,
                        ProductElementID = item.PRODUCTELEMENTID,
                        ProductElementName = item.PRODUCTFORMAT_DETAIL_NAME,
                        ProductElementLength = item.PRODUCTFORMAT_DETAIL_LENTH.ToString(),
                        ProductformatStart=item.PRODUCTFORMAT_START,
                        ProductformatEnd=item.PRODUCTFORMAT_END
                    });
                }
            }

            return new ProductFormatData()
            {
                ID = productFormat.Id,
                Code = productFormat.PRODUCTFORMAT_CODE,
                Name = productFormat.PRODUCTFORMAT_NAME,
                Description = productFormat.PRODUCTFORMAT_DESCRIPTION,
                ProductElements= list
            };

        }

        public virtual PageList<ProductFormatPageResult> GetPageList(ProductFormatSearchModel search)
        {
            return ProductFormatRepository.GetPageList(search);
        }

        /// <summary>
        /// 生成编码方法
        /// </summary>
        /// <param name="dic">传入所有可供使用的变量参数（元素类型code，值）</param>
        /// <param name="code">功能点Code</param>
        /// <param name="number">生成编码数</param>
        /// <returns></returns>
        public List<string> GetNumbers(Dictionary<string, string> dic,string code,int number)
        {
            DateTime dtCurrent=DateTime.Now;//获取系统当前时间

            //查找功能点编码，提示没有功能点
            var setup = SetupRepository.FirstOrDefault(t => t.SETUP_TYPE == "SYS_BUSINESS" && t.SETUP_NO == code);
            if (setup == null)
            {
                throw new BlocksBussnessException("101", L("未查找到相关功能点！"), null);
            }
            //查找功能点对应编码规则，提示没有对应编码规则
            var rule = Product4ElementRuleRepository.FirstOrDefault(t => t.PRODUCT_FUNC_ID == setup.Id);
            if (rule == null || rule.PRODUCT_ELEMENT_RULE_ID == null || rule.PRODUCT_ELEMENT_RULE_ID == "")
            {
                throw new BlocksBussnessException("101", L("功能点未维护对应的编码规则！"), null);
            }
            //查找编码规则
            var productFormat = ProductFormatRepository.FirstOrDefault(t => t.Id == rule.PRODUCT_ELEMENT_RULE_ID);
            if (productFormat == null)
            {
                throw new BlocksBussnessException("101", L("未查找到对应编码规则！"), null);
            }
            //查找编码规则明细
            var productFormatDetail = ProductFormatDetailRepository.GetListByFormatId(productFormat.Id).OrderBy(t=>t.Seq);
            if (!productFormatDetail.Any())
            {
                throw new BlocksBussnessException("101", L("未查找到相关编码规则明细！"), null);
            }
            //编码规则是否符合功能点要求已在功能点对应编码规则维护时判断，此处就不再额外判断

            //判断是否需要生产流水码
            int startIndex = 0;//开始数字
            int increment = 0;//自增量
            if (productFormatDetail.Any(t => t.ProductElementType.Equals("00206")))
            {
                //查找对应明细
                var detail = productFormatDetail.FirstOrDefault(t => t.ProductElementType.Equals("00206"));
                //查找对应元素
                var element = ProductElementRepository.FirstOrDefault(t => t.Id == detail.ProductElementId);
                //通过元素得到其周期
                var setup2 = SetupRepository.FirstOrDefault(t => t.SETUP_TYPE == "RESET_DATE" && t.SETUP_NO == element.RESET_DATE);

                //按流水码、周期生成唯一键
                string indentity = "";
                switch (setup2.SETUP_PARAMETER) {
                    case "0":
                        //无 -- 没有循环周期只是自增
                        indentity = productFormat.PRODUCTFORMAT_CODE + "0";
                        break;
                    case "1":
                        //根据生成的前后缀进行自增
                        foreach (var item in productFormatDetail)
                        {
                            int startNum = int.Parse(item.Start) - 1;
                            int lengthNum = int.Parse(item.End) - startNum;
                            switch (item.ProductElementType)
                            {
                                case "00201": //常量
                                    indentity = indentity + item.Default.Substring(startNum, lengthNum);
                                    break;
                                case "00202": //长年
                                    indentity = indentity + dtCurrent.ToString("yyyy").Substring(startNum, lengthNum);
                                    break;
                                case "00203": //短年
                                    indentity = indentity + dtCurrent.ToString("yy").Substring(startNum, lengthNum);
                                    break;
                                case "00204": //短月
                                    indentity = indentity + dtCurrent.ToString("MM").Substring(startNum, lengthNum);
                                    break;
                                case "00205": //日
                                    indentity = indentity + dtCurrent.ToString("dd").Substring(startNum, lengthNum);
                                    break;
                                case "00206": //流水码
                                    break;
                                case "00207": //一位月
                                    string month = dtCurrent.ToString("MM") == "10"
                                        ? "A"
                                        : dtCurrent.ToString("MM") == "11"
                                            ? "B"
                                            : dtCurrent.ToString("MM") == "12"
                                                ? "C"
                                                : dtCurrent.ToString("MM").Replace("0", "");
                                    indentity = indentity + month.Substring(startNum, lengthNum);
                                    break;
                                case "Material_B":
                                    if (dic.ContainsKey("Material_B"))
                                    {
                                        string variable = dic["Material_B"];
                                        int varlength = variable.Length;
                                        //如果起始值大于变量长度即报错，如果结束值大于变量长度则以变量长度为结束值
                                        if (startNum > varlength)
                                        {
                                            throw new BlocksBussnessException("101", L("截取长度起始值大于变量长度，请检查编码规则设置！"), null);
                                        }
                                        if (lengthNum > varlength)
                                        {
                                            indentity = indentity + variable.Substring(startNum, varlength);
                                        }
                                        else
                                        {
                                            indentity = indentity + variable.Substring(startNum, lengthNum);
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        throw new BlocksBussnessException("101", L("未查找到相关变量数据！"), null);
                                    }
                                case "Supplier_B":
                                    if (dic.ContainsKey("Supplier_B"))
                                    {
                                        string variable = dic["Supplier_B"];
                                        int varlength = variable.Length;
                                        //如果起始值大于变量长度即报错，如果结束值大于变量长度则以变量长度为结束值
                                        if (startNum > varlength)
                                        {
                                            throw new BlocksBussnessException("101", L("截取长度起始值大于变量长度，请检查编码规则设置！"), null);
                                        }
                                        if (lengthNum > varlength)
                                        {
                                            indentity = indentity + variable.Substring(startNum, varlength);
                                        }
                                        else
                                        {
                                            indentity = indentity + variable.Substring(startNum, lengthNum);
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        throw new BlocksBussnessException("101", L("未查找到相关变量数据！"), null);
                                    }
                                case "00210":
                                    break;
                            }
                        }
                        indentity = productFormat.PRODUCTFORMAT_CODE + indentity;
                        break;
                    default:
                        //根据选择的年月日类型进行自增
                        indentity = productFormat.PRODUCTFORMAT_CODE + dtCurrent.ToString(setup2.SETUP_PARAMETER);
                        break;
                }

                if (element.AUTO_INCREMENT == null || element.AUTO_INCREMENT == 0) {
                    increment = 1;
                }
                else {
                    increment = (int)element.AUTO_INCREMENT;
                }
                //if (setup2.SETUP_PARAMETER == "0")
                //{
                //    indentity = productFormat.PRODUCTFORMAT_CODE + "0";
                //} else
                //{
                //    indentity = productFormat.PRODUCTFORMAT_CODE + dtCurrent.ToString(setup2.SETUP_PARAMETER);
                //}

                startIndex = GetSequenceStartIndex(indentity, number, increment);
            }

            List<string> list=new List<string>();
            for (int i = 0; i < number; i++)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in productFormatDetail)
                {
                    int startNum = int.Parse(item.Start) - 1;
                    int lengthNum = int.Parse(item.End) - startNum;
                    switch (item.ProductElementType)
                    {
                        case "00201": //常量
                            sb.Append(item.Default.Substring(startNum, lengthNum));
                            break;
                        case "00202": //长年
                            sb.Append(dtCurrent.ToString("yyyy").Substring(startNum, lengthNum));
                            break;
                        case "00203": //短年
                            sb.Append(dtCurrent.ToString("yy").Substring(startNum, lengthNum));
                            break;
                        case "00204": //短月
                            sb.Append(dtCurrent.ToString("MM").Substring(startNum, lengthNum));
                            break;
                        case "00205": //日
                            sb.Append(dtCurrent.ToString("dd").Substring(startNum, lengthNum));
                            break;
                        case "00206": //流水码
                            sb.Append((startIndex + i * increment).ToString().PadLeft(int.Parse(item.Length.ToString()) , '0').Substring(startNum, lengthNum));
                            break;
                        case "00207": //一位月
                            string month = dtCurrent.ToString("MM") == "10"
                                ? "A"
                                : dtCurrent.ToString("MM") == "11"
                                    ? "B"
                                    : dtCurrent.ToString("MM") == "12"
                                        ? "C"
                                        : dtCurrent.ToString("MM").Replace("0", "");
                            sb.Append(month.Substring(startNum, lengthNum));
                            break;
                        case "Material_B":
                            if (dic.ContainsKey("Material_B"))
                            {
                                string variable = dic["Material_B"];
                                int varlength = variable.Length;
                                //如果起始值大于变量长度即报错，如果结束值大于变量长度则以变量长度为结束值
                                if (startNum > varlength)
                                {
                                    throw new BlocksBussnessException("101", L("截取长度起始值大于变量长度，请检查编码规则设置！"), null);
                                }
                                if (lengthNum > varlength)
                                {
                                    sb.Append(variable.Substring(startNum, varlength));
                                }
                                else
                                { 
                                sb.Append(variable.Substring(startNum, lengthNum));
                                }
                                break;
                            }
                            else
                            {
                                throw new BlocksBussnessException("101", L("未查找到相关变量数据！"), null);
                            }
                        case "Supplier_B":
                            if (dic.ContainsKey("Supplier_B"))
                            {
                                string variable = dic["Supplier_B"];
                                int varlength = variable.Length;
                                //如果起始值大于变量长度即报错，如果结束值大于变量长度则以变量长度为结束值
                                if (startNum > varlength)
                                {
                                    throw new BlocksBussnessException("101", L("截取长度起始值大于变量长度，请检查编码规则设置！"), null);
                                }
                                if (lengthNum > varlength)
                                {
                                    sb.Append(variable.Substring(startNum, varlength));
                                }
                                else
                                {
                                    sb.Append(variable.Substring(startNum, lengthNum));
                                }
                                break;
                            }
                            else
                            {
                                throw new BlocksBussnessException("101", L("未查找到相关变量数据！"), null);
                            }
                        case "00210":
                            break;
                    }
                }
                list.Add(sb.ToString());
            }
           
            return list;
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="indentity">流水号标识码</param>
        /// <param name="number">生成个数</param>
        /// <param name="increment">自增量</param>
        /// <returns></returns>
        public int GetSequenceStartIndex(string indentity, int number,int increment)
        {
            var message = String.Format("******************GetSequenceStartIndex,indentity-{0},number-{1},increment-{2}*********************", indentity, number, increment);
            Ilog.LogInformation(message);
            int startIndex = 1;
            //BdtaQueueRepository.Update(t => t.CATA1 == indentity,  t => new BDTA_QUEUE()
            //{ DATAVERSION = t.DATAVERSION +1 });
            try
            {
                var successFlag = BdtaQueueRepository.Update(t => t.CATA1 == indentity, t => new BDTA_QUEUE() { SNO = t.SNO + number * increment });
                if (successFlag <= 0)
                {
                    BDTA_QUEUE bdtaQueueEntity = new BDTA_QUEUE();
                    bdtaQueueEntity.Id = Guid.NewGuid().ToString();
                    bdtaQueueEntity.CATA1 = indentity;
                    bdtaQueueEntity.SNO = 1 + (number - 1) * increment;
                    var returnId = BdtaQueueRepository.InsertAndGetId(bdtaQueueEntity);
                    if (string.IsNullOrEmpty(returnId))
                    {
                        throw new BlocksBussnessException("101", L("插入流水码初始值失败"), null);
                    }
                }
                else
                {
                    var info = BdtaQueueRepository.FirstOrDefault(t => t.CATA1 == indentity);
                    if (info != null)
                    {
                        startIndex = (int)info.SNO;
                    }
                    else {                    
                        message = String.Format("******************GetSequenceStartIndex,indentity-{0},number-{1},increment-{2}****** info: null ***************", indentity, number, increment);
                        Ilog.LogInformation(message);
                    }
                }
            }
            catch (Exception ex)
            {
                message = String.Format("******************GetSequenceStartIndex,indentity-{0},number-{1},increment-{2}****** error: {3} ***************", indentity, number, increment, ex.Message);
                Ilog.LogInformation(message);
                throw new BlocksBussnessException(L(ex.Message), null);
            }


            //var bdtaQueue = BdtaQueueRepository.FirstOrDefault(t => t.CATA1 == indentity);
            //if (bdtaQueue == null)
            //{
            //    BDTA_QUEUE bdtaQueueEntity=new BDTA_QUEUE();
            //    bdtaQueueEntity.Id = Guid.NewGuid().ToString();
            //    bdtaQueueEntity.CATA1 = indentity;
            //    bdtaQueueEntity.SNO = 1 + (number - 1) * increment;
            //   var returnId=BdtaQueueRepository.InsertAndGetId(bdtaQueueEntity);
            //    if (string.IsNullOrEmpty(returnId))
            //    {
            //        throw new BlocksBussnessException("101", L("插入流水码初始值失败"), null);
            //    }
            //}
            //else
            //{
            //   startIndex =int.Parse((bdtaQueue.SNO + increment).ToString()) ;
            //   BdtaQueueRepository.Update(t=>t.CATA1==indentity,t=>new BDTA_QUEUE(){SNO = bdtaQueue.SNO + number * increment });

            //}

            return startIndex;
        }

        public virtual PageList<ComboboxData> GetComboxList(SearchModel search)
        {
            return ProductFormatRepository.GetComboxList(search);
        }
    }
}