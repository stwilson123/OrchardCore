using System.Collections.Generic;
using BlocksCore.Data.Abstractions.Repository;
using BlocksCore.Data.EF.Paging;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;

namespace BlocksCore.Data.EF.Test.FunctionTest.TestModel
{
    public interface ITestRepository : IRepository<TESTENTITY>
    {
        string GetValue(string value);

        List<TESTENTITY> GetTestEntity2Text();

        PageList<TESTENTITY> GetTestEntityDistinct();


        List<TESTENTITY> GetTESTENTITY3s();
        object FromSql();

        int ExecuteSqlCommand(string id);

        object GetLongIdetifier();

        object SkipAndTake(int page, int pageSize);

        object SkipAndTakeFromSql(int page, int pageSize);


        List<TESTENTITY> GetMultLeftJoin();

        PageList<DtoModel> GetTestPageContainsEmptyString();
    }


    public interface ITest2Repository : IRepository<TESTENTITY2>
    {

    }
    public interface ITestRepository3 : IRepository<TESTENTITY3>
    {
    }



}