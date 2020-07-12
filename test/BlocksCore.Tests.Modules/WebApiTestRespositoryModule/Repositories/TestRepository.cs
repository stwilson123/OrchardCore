using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BlocksCore.Abstractions.Data.Paging;
using BlocksCore.Abstractions.UI.Paging;
using BlocksCore.Data.Abstractions.Entities;
using BlocksCore.Data.Abstractions.Paging;
using BlocksCore.Data.Abstractions.UnitOfWork;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using BlocksCore.Data.Linq;
using BlocksCore.Data.Linq2DB.Repository;
using BlocksCore.Domain.Abstractions;
using LinqToDB;

namespace BlocksCore.Data.EF.Test.FunctionTest.TestModel
{
    public class TestRepository : DBSqlRepositoryBase<TESTENTITY>, ITestRepository
    {
        public TestRepository(IUnitOfWorkManager unitOfwork) : base(unitOfwork)
        {
        }

        public virtual string GetValue(string value)
        {
//            var id = Guid.Parse("DDE679DA-AA68-426D-A6C3-FE66D9725490");
            //            var sql = GetAll().Select(result => new TestEntity() {
            //                  Id = result.Id,
            //                TestEntity2  = new TestEntity2() {  Id      = result.TestEntity2.Id},
            //                   TestEntity3s =  result.TestEntity3s
            //                });
            //            return value;
            // var id = Guid.Parse("DDE679DA-AA68-426D-A6C3-FE66D9725490");
            //var guid = Guid.NewGuid().ToString();
            //var sql = GetContextTable()
            //    .Where(t => t.TestEntity2.Id == guid)
            //    .SelectToList(result => new {
            //    Id = result.Id,
            //    TestEntity2  = new  {   result.TestEntity2.Id},
            //    TestEntity3s =  result.TestEntity3s.Select(aa => new {  aa.Id })
            //});


            return value;
        }

        public List<TESTENTITY> GetTestEntity2Text()
        {
            return GetContextTable().SelectToDynamicList((TESTENTITY t) => new TESTENTITY
            {
                TESTENTITY2 = new TESTENTITY2()
                {
                    Text = t.TESTENTITY2.Text
                }
            });
        }

        public PageList<TESTENTITY> GetTestEntityDistinct()
        {
            return GetContextTable().Paging((TESTENTITY t) => new TESTENTITY
            {
                TESTENTITY2 = new TESTENTITY2()
                {
                    Text = t.TESTENTITY2.Text
                }
            }, new Page() {page = 1, pageSize = 10}, true);
        }

        public PageList<DtoModel> GetTestPageContainsEmptyString()
        {
            return GetContextTable().Paging((TESTENTITY t) => new DtoModel
            {
                 Id =  t.Id,
                  COLNUMINT = t.COLNUMINT
            }, new Page() {page = 2, pageSize = 10,filters = new Group(){ groupOp = "And",rules = new List<IRule>(){ new Rule(){ field = "Id", data = "", op = "cn"}} }});
        }

        public List<TESTENTITY> GetTESTENTITY3s()
        {
            return GetContextTable().SelectToDynamicList((TESTENTITY t) =>
                new TESTENTITY()
                {
                    TESTENTITY3s = t.TESTENTITY3s
                });
        }


        public object FromSql()
        {
            var takeNum = 1;
            var skipNum = 0;

            return Context.FromSql<TestDto>("SELECT TESTENTITY.ID FROM TESTENTITY  INNER JOIN TESTENTITY2  " +
                         "ON TESTENTITY.TESTENTITY2ID = TESTENTITY2.ID ")
                .Skip(skipNum).Take(takeNum)
                .ToList();


//            return FromSqlTemp<TestDto>(this.Table,"SELECT * FROM TESTENTITY  INNER JOIN TESTENTITY2 t2 " +
//                                      "ON TESTENTITY.TESTENTITY2ID = TESTENTITY2.ID WHERE TESTENTITY.ID = '12'")
//
//                .Skip(10).Take(10).ToList();
        }

        public int ExecuteSqlCommand(string id)
        {
            return this.ExecuteSqlCommand("DELETE FROM TESTENTITY WHERE ID = {0}", id);
        }

        public object GetLongIdetifier()
        {
            return GetContextTable().OrderBy(qwertyuioasdfghjklzxcvbnmqwertyuioasdfghjklzxcvbnm =>
                    qwertyuioasdfghjklzxcvbnmqwertyuioasdfghjklzxcvbnm.Id)
                .SelectToDynamicList((TESTENTITY qwertyuioasdfghjklzxcvbnmqwertyuioasdfghjklzxcvbnm) =>
                    new TESTENTITY()
                    {
                        Id = qwertyuioasdfghjklzxcvbnmqwertyuioasdfghjklzxcvbnm.Id
                    });
        }

        public object SkipAndTake(int page, int pageSize)
        {
            return GetContextTable().Paging((TESTENTITY t) => new TESTENTITY() {Id = t.Id}, new Page()
            {
                page = page,
                pageSize = pageSize
            });
        }

        public object SkipAndTakeFromSql(int page, int pageSize)
        {
            return SqlQueryPaging<TestDto>(new Page()
                {
                    pageSize = pageSize,
                    page = page
                }, "SELECT TESTENTITY.ID FROM TESTENTITY  INNER JOIN TESTENTITY2  " +
                   "ON TESTENTITY.TESTENTITY2ID = TESTENTITY2.ID ");
        }

        public List<TESTENTITY> GetMultLeftJoin()
        {
            return GetContextTable()
                .InnerJoin((TESTENTITY t) => t.Id, (TESTENTITY3 testEntity3) => testEntity3.TESTENTITYID)
                .LeftJoin((TESTENTITY t) => t.TESTENTITY2ID, (TESTENTITY2 testEntity2) => testEntity2.Id)
                .SelectToList((TESTENTITY t,TESTENTITY2 testEntity2,TESTENTITY3 testEntity3) => new TESTENTITY()
                {
                    Id = t.Id,
                    COLNUMINT = t.COLNUMINT,
                    TESTENTITY2 = new TESTENTITY2()
                    {
                        Id = testEntity2.Id,
                        CREATER = testEntity2.CREATER
                    } ,
                    TESTENTITY3s = new List<TESTENTITY3>()
                    {
                        new TESTENTITY3()
                        {
                            Id = testEntity3.Id,
                            CREATER = testEntity3.CREATER
                        }
                    }
//                    dtoModel3s = new List<DtoModel3>(){
//                        new DtoModel3()
//                        {
//                            Id = testEntity3.Id,
//                            CREATER = testEntity3.CREATER
//                        }
//                    }
                });
        }
 
    }

    public class TestRepository3 : DBSqlRepositoryBase<TESTENTITY3>, ITestRepository3
    {
        public TestRepository3(IUnitOfWorkManager unitOfWork) : base(unitOfWork)
        {
        }
    }

    class TestDto : IQueryEntity
    {
        public string ID { get; set; }
    }
}