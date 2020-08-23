using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel;
using BlocksCore.Data.Linq2DB.Test.TestConfiguration;
using BlocksCore.Test.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WebApiTestApplication;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Linq2DB.Test.TransactionTest
{
    public class DbServiceTest : IClassFixture<ServiceTestContextFix<WebApiTestApplication.Startup>>, IClassFixture<SpecialDbModelContextFixs>
    {
        private readonly ServiceTestContextFix<WebApiTestApplication.Startup> serviceTestContextFix;
        private readonly SpecialDbModelContextFixs testModelContext;

        public DbServiceTest(ServiceTestContextFix<WebApiTestApplication.Startup> serviceTestContextFix, SpecialDbModelContextFixs testModelContext)
        {
            this.serviceTestContextFix = serviceTestContextFix;
            this.testModelContext = testModelContext;
        }
        [Theory]
        [MultDbData()]
        public async void TestTransactionWhenException(string providerName)
        {


            var guid = Guid.NewGuid().ToString("N");
            var client = this.serviceTestContextFix.Factory.CreateClient();
            var inputObj = new { Id = guid, date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0 };
            var inputString = JsonConvert.SerializeObject(inputObj);
            var resultContent = await client.PostAsync(this.serviceTestContextFix.PrePath+ "/WebApiTestModule/Db/TransactionWhenException", new StringContent(inputString, Encoding.UTF8, "application/json"));

            var result = await resultContent.Content.ReadAsStringAsync();


            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;
            var rep = serviceProvider.GetService<ITestRepository>();
            var nullObj = await rep.FirstOrDefaultAsync(t => t.Id == guid);

            Assert.Null(nullObj);

        }


        [Theory]
        [MultDbData()]
        public async void TestTransaction(string providerName)
        {


            var guid = Guid.NewGuid().ToString("N");
            var client = this.serviceTestContextFix.Factory.CreateClient();
            var inputObj = new { Id = guid, date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0,str = Guid.NewGuid().ToString("N") };
            var inputString = JsonConvert.SerializeObject(inputObj);
            var resultContent = await client.PostAsync(this.serviceTestContextFix.PrePath + "/WebApiTestModule/Db/TransactionSuccess", new StringContent(inputString, Encoding.UTF8, "application/json"));

            var result = await resultContent.Content.ReadAsStringAsync();


            var serviceProvider = this.testModelContext.testModelContexts.FirstOrDefault(ctx => ctx.ProviderName == providerName).ServiceProvider;
            var rep = serviceProvider.GetService<ITestRepository>();
            var insertObj = await rep.FirstOrDefaultAsync(t => t.Id == guid);

            Assert.NotNull(insertObj);
            Assert.Equal(inputObj.str, insertObj.STRING);

        }
    }
}
