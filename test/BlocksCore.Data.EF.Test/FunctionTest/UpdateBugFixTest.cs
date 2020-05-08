using System.Collections.Generic;
using System.Linq;
using BlocksCore.Data.EF.Test.FunctionTest.TestModel;
using BlocksCore.Data.EF.Test.TestConfiguration;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlocksCore.Data.EF.Test.FunctionTest
{
    public class UpdateBugFixTest : IClassFixture<DbModelContextFixs>
    {
        private readonly DbModelContextFixs testModelContext;

        public UpdateBugFixTest(DbModelContextFixs testModelContext)
        {
            this.testModelContext = testModelContext;
        }
        [Theory]
        [MultDbData()]
        public void update_multColumn_calculation_shouldnot_throw_exception(string providerName)
        {
            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;
            var rep = serviceProvider.GetService<ITestRepository>();



            //var trans = rep.Context.Database.BeginTransaction();执行时间


            var inputData = new inputData() { input1 = 1, input2 = 2 };

            Dictionary<string, string> dic = new Dictionary<string, string>();

            rep.Update(
                t => t.Id == "" && ((t.ISACTIVE + inputData.input1) < t.ISACTIVE) &&
                     ((t.COLNUMINT + inputData.input2) < t.COLNUMINT_NULLABLE), t => new TESTENTITY()
                     {
                         ISACTIVE = t.ISACTIVE + inputData.input1,
                         COLNUMINT = t.COLNUMINT + inputData.input2

                     });


            //trans.Commit();
        }

        //[Fact]
        //public void update_oncurrent()
        //{

        //    var rep = Resolve<ITestRepository>();

        //    var unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        //    var uow = unitOfWorkManager.Begin();

        //    //var trans = rep.Context.Database.BeginTransaction();执行时间
        //    var a = rep.FirstOrDefault(t => t.Id == "109649d7-f0b1-4518-8991-9fe3c1dde6ce");

        //    var rows = rep.Update(t => t.Id == "109649d7-f0b1-4518-8991-9fe3c1dde6ce" && t.COLNUMINT > 600, t => new TESTENTITY()
        //    {

        //        COLNUMINT = t.COLNUMINT - 600

        //    });
        //    var a1 = rep.FirstOrDefault(t => t.Id == "109649d7-f0b1-4518-8991-9fe3c1dde6ce");
        //    uow.Complete();
        //    //trans.Commit();
        //}

        //[Fact]
        //public void update_oncurrent1()
        //{
        //    var unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        //    var rep = Resolve<ITestRepository>();

        //    var uow = unitOfWorkManager.Begin();


        //    //var trans = rep.Context.Database.BeginTransaction();执行时间

        //    var rows = rep.Update(t => t.Id == "109649d7-f0b1-4518-8991-9fe3c1dde6ce" && t.COLNUMINT > 600, t => new TESTENTITY()
        //    {

        //        COLNUMINT = t.COLNUMINT - 600

        //    });
        //    var a = rep.FirstOrDefault(t => t.Id == "109649d7-f0b1-4518-8991-9fe3c1dde6ce");

        //    uow.Complete();
        //    //trans.Commit();
        //}


    }

    class inputData
    {
        public int input1 { get; set; }
        public int input2 { get; set; }

    }
}