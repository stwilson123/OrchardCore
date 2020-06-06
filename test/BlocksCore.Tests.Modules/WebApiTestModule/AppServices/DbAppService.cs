using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.EF.Test.FunctionTest.TestModel;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;

namespace WebApiTestModule.AppServices
{
    public class DbAppService : IDbAppService
    {
        private readonly ITestRepository testRepository;

        public DbAppService(ITestRepository testRepository)
        {
            this.testRepository = testRepository;
        }
        public string TransactionSuccess(dynamic inputData)
        {

            testRepository.Insert(new TESTENTITY()
            {
                Id = inputData.Id,
            });
            throw new Exception();
        }
    }
}
