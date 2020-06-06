using System;
using System.Net.Http;
using System.Text;
using BlocksCore.Test.Abstractions;
using BlocksCore.Web.Abstractions.Result;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WebApiTestApplication;
using Xunit;

namespace BlocksCore.WebApi.Test
{
    public class ControllerTest : IClassFixture<ServiceTestContextFix<Startup>>
    {
        private readonly ServiceTestContextFix<Startup> serviceTestContextFix;
        
        public ControllerTest(ServiceTestContextFix<Startup> serviceTestContextFix)
        {
            this.serviceTestContextFix = serviceTestContextFix;
        }

        [Fact]
        public async void Test1()
        {
            var client = serviceTestContextFix.Factory.CreateClient();
            var inputObj = new {date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0};
            var inputString = JsonConvert.SerializeObject(inputObj);
            var resultContent = await client.PostAsync("/WebApiTestModule/Admin/DefaultMethodFromObject", new StringContent(inputString, Encoding.UTF8,"application/json"));

            var result = await resultContent.Content.ReadAsStringAsync();

            Assert.Equal(serviceTestContextFix.JsonConvert.SerializeObject(new DataResult() { Code = "200", Content = inputObj }), result);

        }
    }
}