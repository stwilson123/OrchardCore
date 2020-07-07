using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blocks.BussnessDTOModule;
using Blocks.BussnessDTOModule.MasterData;
using Blocks.BussnessEntityModule;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.EF.Linq;
using BlocksCore.Data.EF.Repository;

namespace Blocks.BussnessRespositoryModule
{
    public class TestRepository : DBSqlRepositoryBase<TESTENTITY>, ITestRepository
    {
        public TestRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public string GetValue(string value)
        {
          
            //            var id = Guid.Parse("DDE679DA-AA68-426D-A6C3-FE66D9725490");
            //            var sql = GetAll().Select(result => new TestEntity() {
            //                  Id = result.Id,
            //                TestEntity2  = new TestEntity2() {  Id      = result.TestEntity2.Id},
            //                   TestEntity3s =  result.TestEntity3s
            //                });
            //            return value;
            // var id = Guid.Parse("DDE679DA-AA68-426D-A6C3-FE66D9725490");
            //var sql = GetContextTable()

            //    .SelectToList(result => new {
            //        Id = result.Id,
            //        TestEntity2 = new { result.TESTENTITY2.Id },
            //    TestEntity3s = result.TESTENTITY3s
            //});
            return value;
            
        }

        public virtual string GetValueOverride(string value)
        {
          
            throw new NotImplementedException();
        }

        public PageList<PageResult> GetPageList(SearchModel search)
        {

            return   GetContextTable()
                
              //.InnerJoin((TESTENTITY2 testEntity2) => testEntity2.Id,(TESTENTITY testEntity) => testEntity.TESTENTITY2ID)
              .Paging((TESTENTITY testEntity) => new PageResult
              {
                  Id = testEntity.Id,
                  comboboxId = testEntity.TESTENTITY2.Id,
                  comboboxText = testEntity.TESTENTITY2.Text,
                  city = testEntity.STRING,
                  isActive = testEntity.ISACTIVE,
                  comment = testEntity.COMMENT,
                  registerTime = testEntity.REGISTERTIME,
                  num = 100
              },search.page);
            //return GetContextTable()
            //     //.InnerJoin((TESTENTITY2 testEntity2) => testEntity2.Id,(TESTENTITY testEntity) => testEntity.TESTENTITY2ID)
            //    .Paging((TESTENTITY testEntity) => new PageResult
            //    {
            //        Id = testEntity.Id,
            //        comboboxId = testEntity.TESTENTITY2.Id,
            //        comboboxText = testEntity.TESTENTITY2.Text,
            //        city = testEntity.STRING,
            //        isActive = testEntity.ISACTIVE,
            //        comment = testEntity.COMMENT,
            //        registerTime =   testEntity.REGISTERTIME
            //    }, search.page);
 
        }


        public List<PageResult> GetList()
        {

            return GetContextTable()
                .InnerJoin((TESTENTITY2 testEntity2) => testEntity2.Id, (TESTENTITY testEntity) => testEntity.TESTENTITY2ID)
                .OrderBy((testEntity) => new { testEntity.CREATER,testEntity.DATAVERSION })
                .ThenBy((TESTENTITY2 testEntity2) => new { testEntity2.CREATER, testEntity2.DATAVERSION  })
                //
                .SelectToList((TESTENTITY testEntity) => new PageResult
                {
                    Id = testEntity.Id,
                    comboboxId = testEntity.TESTENTITY2.Id,
                    comboboxText = testEntity.TESTENTITY2.Text,
                    city = testEntity.STRING,
                    isActive = testEntity.ISACTIVE,
                    comment = testEntity.COMMENT,
                    registerTime = testEntity.REGISTERTIME
                });
            //            return GetContextTable().InnerJoin((TESTENTITY testEntity) => testEntity.TESTENTITY2ID,
            //                    (TESTENTITY2 testEntity2) => testEntity2.Id)
            //                .Paging((TESTENTITY testEntity,TESTENTITY2 testEntity2) => new PageResult
            //                {
            //                    Id = testEntity.Id,
            //                    comboboxText = testEntity2.Text,
            //                    city = testEntity.STRING,
            //                    isActive = testEntity.ISACTIVE,
            //                    comment = testEntity.COMMENT,
            //                     registerTime =   testEntity.REGISTERTIME
            //                }, search.page);
        }
    }
}