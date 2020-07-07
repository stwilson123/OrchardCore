using System;
using System.Collections.Generic;
using System.Linq;
//using Blocks.BussnessDomainModule.RPC;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using Blocks.BussnessEntityModule;
using Blocks.BussnessRespositoryModule;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Event.Abstractions;
using Blocks.Framework.Utility.TypeHelper;


namespace Blocks.BussnessDomainModule.MasterData
{
    public class MasterDataDomainEvent : IDomainService
    {
        public MasterDataDomainEvent(ITestRepository testRepository, ITest2Repository test2Repository, IStringLocalizer<MasterDataDomainEvent> l)
        {

            this.testRepository = testRepository;
            this.test2Repository = test2Repository;
            L = l;
        }

        public IEventBus EventBus { get; set; }

        public IStringLocalizer L { get; set; }

       // public TestRPC testRPC { get; set; }

        private ITestRepository testRepository { get; set; }

        private ITest2Repository test2Repository { get; set; }


        public virtual string Add(MasterData data)
        {
            var newMasterData = new TESTENTITY()
            {
                STRING = data.city,
                ISACTIVE = SafeConvert.ToInt64(data.isActive),
                COMMENT = data.comment,
                TESTENTITY2ID = data.combobox,
                REGISTERTIME =data.registerTime,
                
            };
            var Id = testRepository.Insert(newMasterData).Id;
            return Id;
//            testRepository.Update(t => t.Id == "57627fde-0332-4db0-9036-ce3ec5e48496", t => 
//                new TESTENTITY()
//                {
//                    
//                    COMMENT = DateTime.Now.ToString("HH:mm:ss")
//                }
//            );
            //throw new Exception();
            //test2Repository.Update(t => t.Id == "123",t => new TESTENTITY2() {
            //     Text = DateTime.Now.ToString("HH:mm:ss tt zz")

            //});
         
            //return null;
        }

        public virtual MasterData Get(string Id)
        {
            
            var entity = testRepository.FirstOrDefault(Id);
            
            if(entity == null)
                throw new BlocksBussnessException("101", L["Not Found"], null);
            var newMasterData = new MasterData()
            {
                Id = entity.Id,
                city=  entity.STRING,
                isActive = entity.ISACTIVE == 0 ? false :true,  
                comment = entity.COMMENT,
                combobox = entity.TESTENTITY2ID,
                registerTime =entity.REGISTERTIME,
                
            };
            return newMasterData;
//            testRepository.Update(t => t.Id == "57627fde-0332-4db0-9036-ce3ec5e48496", t => 
//                new TESTENTITY()
//                {
//                    
//                    COMMENT = DateTime.Now.ToString("HH:mm:ss")
//                }
//            );
            //throw new Exception();
            //test2Repository.Update(t => t.Id == "123",t => new TESTENTITY2() {
            //     Text = DateTime.Now.ToString("HH:mm:ss tt zz")

            //});
         
            //return null;
        }


        public virtual  PageList<PageResult>  GetPageList(SearchModel search)
        {
            EventBus.Trigger(new TaskEventData {id = "123123"});

            var a =  testRepository.GetPageList(search);
            return a;
        }


        public virtual void TestException()
        {
            throw new BlocksBussnessException("101", L["TestException"], null);

        }

        //public virtual string ProxTest(string input)
        //{
        //    return testRPC.ProxFunction(new RPC.ProxModel() { dic = new Dictionary<string, string>() {

        //        { input,input}
        //    } }).FirstOrDefault();
        //}
    }
}