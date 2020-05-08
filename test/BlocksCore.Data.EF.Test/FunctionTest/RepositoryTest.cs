using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BlocksCore.Data.EF.Test.FunctionTest.TestModel;
using BlocksCore.Data.EF.Test.TestConfiguration;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace BlocksCore.Data.EF.Test.FunctionTest
{

    public class RepositoryTest : IClassFixture<DbModelContextFixs>
    {
        private readonly DbModelContextFixs testModelContext;
        //IServiceCollection services;
        //IServiceProvider serviceProvider;
        private readonly ITestOutputHelper outputHelper;
        public RepositoryTest(DbModelContextFixs testModelContext, ITestOutputHelper outputHelper)
        {

            this.testModelContext = testModelContext;
            //this.services = testModelContext.Services;
            //this.serviceProvider = testModelContext.ServiceProvider;
            this.outputHelper = outputHelper;
        }

        //[Fact]Entry
        [Theory]
        [MultDbData()]
        public async void BatchAddPerformance(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;
            var rep = serviceProvider.GetService<ITestRepository>();


            //var trans = rep.Context.Database.BeginTransaction();执行时间
            var listTestEntity = new List<TESTENTITY>();
            for (int i = 0; i < 10000; i++)
            {
                listTestEntity.Add(new TESTENTITY() { Id = Guid.NewGuid().ToString(), COLNUMINT = i, UPDATER = "1", CREATER = "1"});
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            rep.Insert(listTestEntity);
            stopwatch.Stop();

            this.outputHelper.WriteLine("Sync Insert Total Milliseconds:" + stopwatch.ElapsedMilliseconds);

            //var trans = rep.Context.Database.BeginTransaction();执行时间
            listTestEntity = new List<TESTENTITY>();
            for (int i = 0; i < 10000; i++)
            {
                listTestEntity.Add(new TESTENTITY() { Id = Guid.NewGuid().ToString(), COLNUMINT = i, UPDATER = "1", CREATER = "1" });
            }
            stopwatch.Restart();
            await rep.InsertAsync(listTestEntity);
            stopwatch.Stop();

            this.outputHelper.WriteLine("Async Insert Total Milliseconds:" + stopwatch.ElapsedMilliseconds);

            //Assert.True(false, "Total Milliseconds:" + stopwatch.ElapsedMilliseconds);
            //trans.Commit();
        }
        [Theory]
        [MultDbData()]
        public async void SyncQueryMethod(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;
            var rep = serviceProvider.GetService<ITestRepository>();

            rep.Insert(new TESTENTITY() { Id = Guid.NewGuid().ToString() });

            var firstData = rep.GetAllList().FirstOrDefault();
            Assert.True(firstData.Id == rep.Get(firstData.Id).Id);
            Assert.True(firstData.Id == rep.FirstOrDefault(firstData.Id).Id);
            Assert.True(firstData.Id == rep.FirstOrDefault(t => t.Id == firstData.Id).Id);

            Assert.True(firstData.Id == rep.Single(t => t.Id == firstData.Id).Id);

            Assert.True(1 == rep.Count(t => t.Id == firstData.Id));

            Assert.True(1 == rep.LongCount(t => t.Id == firstData.Id));



            Assert.True(firstData.Id == (await rep.GetAsync(firstData.Id)).Id);
            Assert.True(firstData.Id == (await rep.FirstOrDefaultAsync(firstData.Id)).Id);
            Assert.True(firstData.Id == (await rep.FirstOrDefaultAsync(t => t.Id == firstData.Id)).Id);

            Assert.True(firstData.Id == (await rep.SingleAsync(t => t.Id == firstData.Id)).Id);

            Assert.True(1 == (await rep.CountAsync(t => t.Id == firstData.Id)));

            Assert.True(1 == await rep.LongCountAsync(t => t.Id == firstData.Id));

        }


        [Theory]
        [MultDbData()]
        public async void Query_Shouldnot_Throw_LongIdetifier(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();

            var qwertyuioasdfghjklzxcvbnmqwertyuioasdfghjklzxcvbnm = "123";
            rep.FirstOrDefault(t => t.Id == qwertyuioasdfghjklzxcvbnmqwertyuioasdfghjklzxcvbnm);

            rep.GetLongIdetifier();

            await rep.FirstOrDefaultAsync(t => t.Id == qwertyuioasdfghjklzxcvbnmqwertyuioasdfghjklzxcvbnm);

        }
        [Theory]
        [MultDbData()]
        public async void UpdateByModel_Should_GetSameEntity(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();
            var id= rep.InsertAndGetId(new TESTENTITY(){ Id = Guid.NewGuid().ToString(), TESTENTITY2ID = Guid.NewGuid().ToString()});
            var testEntity = rep.FirstOrDefault(t => t.Id != null);
            var setGuid = Guid.NewGuid().ToString();
            testEntity.STRING = setGuid;
            rep.Update(testEntity);

            Assert.Equal(setGuid, rep.FirstOrDefault(r => r.Id == id).STRING);
            Assert.Equal(setGuid, rep.Single(r => r.Id == id).STRING);



            //async test
            id = await rep.InsertAndGetIdAsync(new TESTENTITY() { Id = Guid.NewGuid().ToString(), TESTENTITY2ID = Guid.NewGuid().ToString() });
            testEntity = await rep.FirstOrDefaultAsync(t => t.Id != null);
            setGuid = Guid.NewGuid().ToString();
            testEntity.STRING = setGuid;
            await rep.UpdateAsync(testEntity);

            Assert.Equal(setGuid, rep.FirstOrDefault(r => r.Id == id).STRING);
            Assert.Equal(setGuid, rep.Single(r => r.Id == id).STRING);
        }

        [Theory]
        [MultDbData()]
        public void UpdateByExpression_Should_GetSameEntity(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();
            var guid = Guid.NewGuid().ToString();

            var now = DateTime.Now;
            var initData =new TESTENTITY() {Id = guid, TESTENTITY2ID = "guid" + "2", COLNUMINT = 1, ISACTIVE = 1} ;
            rep.Insert(initData);

            rep.Update(rr => rr.Id == guid, RR => new TESTENTITY()
            {
                TESTENTITY2ID ="guid" + "2"  ,

            });

            var constValue = "123";
            var id = rep.Update(rr => rr.Id == guid && rr.CREATEDATE <= now, RR => new TESTENTITY()
            {
                TESTENTITY2ID = RR.TESTENTITY2ID + constValue  ,
                COMMENT = "123321"

            });
            var updatedData = rep.FirstOrDefault(t => t.Id == guid);
            Assert.Equal(updatedData.TESTENTITY2ID,initData.TESTENTITY2ID + "123"  );

            var inputPlus = "inputPlus";
            var id1 = rep.Update(t => t.Id == guid, t => new TESTENTITY() { TESTENTITY2ID = t.TESTENTITY2ID + inputPlus });

            Assert.Equal(rep.FirstOrDefault(t => t.Id == guid).TESTENTITY2ID,updatedData.TESTENTITY2ID +inputPlus);

            var testEntity = rep.FirstOrDefault(t => t.Id == guid);
            var setGuid = Guid.NewGuid().ToString();
            testEntity.TESTENTITY2ID = setGuid;
            rep.Update(testEntity);
            Assert.Equal(testEntity.TESTENTITY2ID,setGuid);


        }

        [Theory]
        [MultDbData()]
        public void UpdateByExpression_Fixbug(string providerName)
        {

            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();
            var guid = Guid.NewGuid().ToString();

            var now = DateTime.Now;
            var initData =new TESTENTITY() {Id = guid, TESTENTITY2ID = "guid" + "2", COLNUMINT = 1, ISACTIVE = 1} ;
            rep.Insert(initData);

            var constValue = "123";


            var rowIds = rep.Update(rr => rr.Id == "1231232132132132132131233213123" && rr.CREATEDATE <= now, RR => new TESTENTITY()
            {
                TESTENTITY2ID = RR.TESTENTITY2ID + initData.TESTENTITY2ID  ,
                COMMENT = "123321"
            });
            var id = rep.Update(rr => rr.Id == guid && rr.CREATEDATE <= now, RR => new TESTENTITY()
            {
                TESTENTITY2ID = RR.TESTENTITY2ID + initData.TESTENTITY2ID  ,
                COMMENT = "123321"
            });
            var updatedData = rep.FirstOrDefault(t => t.Id == guid);

            Assert.Equal(updatedData.TESTENTITY2ID,initData.TESTENTITY2ID + initData.TESTENTITY2ID  );




        }


        [Theory]
        [MultDbData()]
        public void queryCombination(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();
            var testEntity = rep.GetAllList();
            var tes =  rep.GetValue("123");

        }


        [Theory]
        [MultDbData()]
        public void QueryBySqlWithArbitaryType(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();

            var a = rep.FromSql();


        }


        [Theory]
        [MultDbData()]
        public void DeleteByExpression(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();
            var guid = Guid.NewGuid().ToString();
            rep.Insert(new TESTENTITY() {Id = guid, TESTENTITY2ID = guid, COLNUMINT = 1, ISACTIVE = 1});

            Assert.NotNull(rep.FirstOrDefault(t => t.Id == guid ));
             rep.Delete(t => t.Id == guid );

            Assert.Null(rep.FirstOrDefault(t => t.Id == guid ));


        }



        [Theory]
        [MultDbData()]
        public void MultRepoFetch(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();

            var rep3 = serviceProvider.GetService<ITestRepository3>();
            rep.FirstOrDefault(t => t.Id == "");

            rep3.FirstOrDefault(t => t.Id == "");
        }


        [Theory]
        [MultDbData()]
        public void PageNotToThrowExceptionInOracle11gandsqlserver2008(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();
            rep.SkipAndTakeFromSql(1, 10);

            rep.SkipAndTake(1,10);


        }


        [Theory]
        [MultDbData()]
        public void InsertOrUpdateTest(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep = serviceProvider.GetService<ITestRepository>();
            Assert.Throws<NotImplementedException>(() => { rep.InsertOrUpdate(new TESTENTITY() {Id = "123"}); });



        }



        [Theory]
        [MultDbData()]
        public  void ExcuteSql(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var rep =serviceProvider.GetService<ITestRepository>();
            Assert.Equal(0,rep.ExecuteSqlCommand(Guid.NewGuid().ToString()));
            var newId = Guid.NewGuid().ToString();
            rep.Insert(new TESTENTITY()
            {
                Id = newId
            });
            Assert.Equal(1,rep.ExecuteSqlCommand(newId));


        }



        [Theory]
        [MultDbData()]
        public void TestUpdateMethodWhereExpression_listContainsExpression_throw_exception(string providerName)
        {

//            var rep = Resolve<ITestRepository>();
//            var list = new List<string>(){ "123"};
//            rep.Update(t => list.Contains(t.Id) && t.COMMENT == null, testentity => new TESTENTITY()
//            {
//                COLNUMINT = 1
//
//            });

            //var rep = Resolve<IWhMaterialBatchRespository>();
            //IEnumerable<string> lotNos = new List<string>(){ "2019112000001-LX150001","TTT51877T" }.Distinct();
            //rep.Update(t =>lotNos.Contains(t.MATERIAL_BATCH) && t.DATE_INSTORAGE == null, testentity => new WH_MATERIAL_BATCH()
            //{
            //    DATE_INSTORAGE = DateTime.Now
            //});


            //lotNos = new List<string>(){ "2019112000001-LX150001","TTT51877T" };
            //rep.Update(t =>lotNos.Contains(t.MATERIAL_BATCH) && t.DATE_INSTORAGE == null, testentity => new WH_MATERIAL_BATCH()
            //{
            //    DATE_INSTORAGE = DateTime.Now
            //});
        }


    }


}