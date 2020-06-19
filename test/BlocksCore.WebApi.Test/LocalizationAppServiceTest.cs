using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Test.Abstractions;
using Xunit;
using WebApiTestApplication;
using System.Net.Http;
using Newtonsoft.Json;
using BlocksCore.Web.Abstractions.Result;

namespace BlocksCore.WebApi.Test
{
    public class LocalizationAppServiceTest : IClassFixture<ServiceTestContextFix<Startup>>
    {
        private readonly ServiceTestContextFix<Startup> serviceTestContextFix;

        public LocalizationAppServiceTest(ServiceTestContextFix<Startup> serviceTestContextFix)
        {
            this.serviceTestContextFix = serviceTestContextFix;
        }
        [Fact]
        public async void TestGetLocalzedStringFromAppService()
        {
            var client = this.serviceTestContextFix.Factory.CreateClient();
            var result = await client.PostAsync(this.serviceTestContextFix.PrePath + "/WebApiTestModule/Localization/GetLocalzedString", new StringContent("", Encoding.UTF8, "application/json"));
            var resultContent = await result.Content.ReadAsStringAsync();

            var resultObj = JsonConvert.DeserializeObject<DataResult<IDictionary<string, string>>>(resultContent);

            Assert.NotNull(resultObj);
            Assert.NotNull(resultObj.Content);

            Assert.Equal("你好", resultObj.Content["hello"]);
            Assert.Equal("notfound", resultObj.Content["notFound"]);

            

        }
}
}
