using System;
using System.Collections.Generic;
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
    public class ResultTest : IClassFixture<ServiceTestContextFix<Startup>>
    {
        private readonly ServiceTestContextFix<Startup> serviceTestContextFix;
        object inputObject = new { date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0 };
        string inputString;
        JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };
        public ResultTest(ServiceTestContextFix<Startup> serviceTestContextFix)
        {
            inputString = JsonConvert.SerializeObject(inputObject);
            this.serviceTestContextFix = serviceTestContextFix;
        }

        [Fact]
        public async void TestResult()
        {
            var client = this.serviceTestContextFix.Factory.CreateClient();

            var resultContent = await client.PostAsync(this.serviceTestContextFix.PrePath + "/WebApiTestModule/Result/GetObject", new StringContent(inputString, Encoding.UTF8, "application/json"));
            var result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, this.serviceTestContextFix.JsonConvert.SerializeObject(new DataResult() { Code = "200", Content = inputObject }));

            var inputValue = "123";
            resultContent = await client.GetAsync(this.serviceTestContextFix.PrePath + "/WebApiTestModule/Result/GetValue?value="+ inputValue);
            result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, this.serviceTestContextFix.JsonConvert.SerializeObject(new DataResult() { Code = "200", Content = inputValue }));
        }


        [Fact]
        public async void TestResultWhenException()
        {
            var client = this.serviceTestContextFix.Factory.CreateClient();

            var resultContent = await client.PostAsync(this.serviceTestContextFix.PrePath + "/WebApiTestModule/Result/GetObjectWhenException", new StringContent(inputString, Encoding.UTF8, "application/json"));
            var result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, this.serviceTestContextFix.JsonConvert.SerializeObject(new DataResult() { Code = "500", Content = null, Msg = new NotImplementedException().Message }));


            resultContent = await client.PostAsync(this.serviceTestContextFix.PrePath + "/WebApiTestModule/Result/GetObjectWhenBlocksException", new StringContent(inputString, Encoding.UTF8, "application/json"));
            result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, this.serviceTestContextFix.JsonConvert.SerializeObject(new DataResult() { Code = "101", Content = inputObject, Msg = "BlocksException" }));
        }
    }
}
