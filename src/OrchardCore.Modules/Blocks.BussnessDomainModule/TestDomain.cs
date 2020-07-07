using System;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using Blocks.BussnessEntityModule;
using Blocks.BussnessRespositoryModule;
using BlocksCore.AutoMapper.Abstractions;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Domain.Abstractions.Domain;
using BlocksCore.Event.Abstractions;

namespace Blocks.BussnessDomainModule
{
    public class TestDomain : IDomainService
    {
        public IEventBus EventBus { get; set; }
        public TestDomain(ITestRepository testRepository)
        {
            this.testRepository = testRepository;
        }

        private ITestRepository testRepository { get; set; }

        public virtual string GetValue(string value)
        {
            EventBus.Trigger(new TaskEventData {id = "123123"});
            testRepository.FirstOrDefault(t => t.Id == "123");
            return testRepository.GetValue(value);
        }
        

        public virtual string GetValueOverride()
        {
            return testRepository.GetValueOverride("");
        }


        public virtual string Add(MasterData.MasterData data)
        {

            var newMasterData =  data.AutoMapTo<TESTENTITY>();
            return testRepository.Insert(newMasterData).Id;
        }

        public virtual string Update(MasterData.MasterData data)
        {

            
            return testRepository.Update(t => t.Id == "123123",t => new TESTENTITY { STRING = "123" }).ToString();
        }

        public virtual string GetList(string value)
        {
            EventBus.Trigger(new TaskEventData {id = "123123"});
            testRepository.FirstOrDefault(t => t.Id == "123");
            return testRepository.GetValue(value);
        }
        
        
        public virtual  PageList<PageResult>  GetPageList(SearchModel search)
        {
            var a =  testRepository.GetPageList(search);
            return a;
        }
    }
    
    public class TaskEventData : EventData
    {
        public string id { get; set; }
    } 
}