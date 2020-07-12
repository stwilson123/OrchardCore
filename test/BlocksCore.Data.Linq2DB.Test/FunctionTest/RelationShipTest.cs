using System.Collections.Generic;
using System.Linq;
using BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel;
using BlocksCore.Data.Linq2DB.Test.TestConfiguration;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Linq2DB.Test.FunctionTest
{
    public class RelationShipTest : IClassFixture<DbModelContextFixs>
    {
        private readonly DbModelContextFixs testModelContext;

        public RelationShipTest(DbModelContextFixs testModelContext)
        {
            this.testModelContext = testModelContext;
        }

        [Theory]
        [MultDbData()]
        public void OneToOneMethod(string providerName)
        {

            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;

            var list = new List<MyClassModel>();
            var list2 = new List<MyClassModel2>();

            //            var groupList = list.GroupJoin(list2,t => t.MyClass2Id, p => p.id, (inner, outer) => new { inner, outer} )
            //                .SelectMany(joinResult => joinResult.outer.DefaultIfEmpty(),
            //                    (a,b) => {
            //                        a.inner,
            //                       b
            //            }
            //            )
            var rep = serviceProvider.GetService<ITestRepository>();


            var firstData2 = rep.GetMultLeftJoin();



            var firstData = rep.GetTestEntity2Text();

            var firstData1 = rep.GetTESTENTITY3s();




            // var firstData2 = Resolve<TestRepository3>().GetContextTable().SelectToDynamicList((TESTENTITY3 t) => t.TESTENTITY);
        }

    }

    class MyClassModel
    {
        public string id { get; set; }

        public string MyClass2Id { get; set; }

    }

    class MyClassModel2

    {
        public string id { get; set; }

    }
}