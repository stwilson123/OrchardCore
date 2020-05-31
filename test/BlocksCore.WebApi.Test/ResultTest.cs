using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using BlocksCore.Web.Abstractions.Result;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WebApiTestApplication;
using Xunit;

namespace BlocksCore.WebApi.Test
{
    public class ResultTest
    {
        static string prePath = "/api/services";
        private WebApplicationFactory<Startup> _factory;
        object inputObject = new { date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0 };
        string inputString;
        JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };
        public ResultTest()
        {
            _factory = new WebApplicationFactory<Startup>();
            inputString = JsonConvert.SerializeObject(inputObject);
        }

        [Fact]
        public async void TestResult()
        {
            var client = _factory.CreateClient();

            var resultContent = await client.PostAsync(prePath + "/WebApiTestModule/Result/GetObject", new StringContent(inputString, Encoding.UTF8, "application/json"));
            var result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, JsonConvert.SerializeObject(new DataResult() { Code = "200", Content = inputObject }, JsonSerializerSettings));

            var inputValue = "123";
            resultContent = await client.GetAsync(prePath + "/WebApiTestModule/Result/GetValue?value="+ inputValue);
            result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, JsonConvert.SerializeObject(new DataResult() { Code = "200", Content = inputValue }, JsonSerializerSettings));
        }


        [Fact]
        public async void TestResultWhenException()
        {
            var client = _factory.CreateClient();

            var resultContent = await client.PostAsync(prePath + "/WebApiTestModule/Result/GetObjectWhenException", new StringContent(inputString, Encoding.UTF8, "application/json"));
            var result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, JsonConvert.SerializeObject(new DataResult() { Code = "500", Content = null, Msg = new NotImplementedException().Message }, JsonSerializerSettings));


            resultContent = await client.PostAsync(prePath + "/WebApiTestModule/Result/GetObjectWhenBlocksException", new StringContent(inputString, Encoding.UTF8, "application/json"));
            result = await resultContent.Content.ReadAsStringAsync();
            Assert.Equal(result, JsonConvert.SerializeObject(new DataResult() { Code = "101", Content = inputObject, Msg = "BlocksException" }, JsonSerializerSettings));
        }
    }
}
