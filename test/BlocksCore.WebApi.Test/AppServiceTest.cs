using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApiTestApplication;
using Xunit;

namespace BlocksCore.WebApi.Test
{
    public class AppServiceTest
    {
        static string prePath = "/api/services";
        private WebApplicationFactory<Startup> _factory;
        public AppServiceTest()
        {
            _factory = new WebApplicationFactory<Startup>();

        }

        [Fact]
        public async void TestDataInputAndReturn()
        {
            await TestSendSimpleData(HttpMethod.Get);
            await TestSendDataAndReturn(HttpMethod.Post.Method, "Default");
            await TestSendDataAndReturn(HttpMethod.Get.Method,"Post");
            await TestSendDataAndReturn(HttpMethod.Post.Method);
        }


        [Fact]
        public async void TestMethodNotFound()
        {
            var client = _factory.CreateClient();
            var result = await client.GetAsync(prePath + "/WebApiTestModule/Normal/SpecialMethod");
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);

            result = await client.PostAsync(prePath+"/WebApiTestModule/Normal/GetMethod",null);
            resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, result.StatusCode);
        }


        [Fact]
        public async void TestDefaultModeMethodCanUseAllHttpMethod()
        {
            var client = _factory.CreateClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, prePath+"/WebApiTestModule/Normal/DefaultMethod") {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
           
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.SendAsync(httpRequest);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
 
            result = await client.PostAsync(prePath + "/WebApiTestModule/Normal/DefaultMethod", new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            result = await client.PutAsync(prePath + "/WebApiTestModule/Normal/DefaultMethod",   new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
         
        }

        private async Task TestSendSimpleData(HttpMethod httpMethod, string methodName = null)
        {
            if (string.IsNullOrEmpty(methodName))
                methodName = httpMethod.Method;
            var client = _factory.CreateClient();
            var inputObj = new Dictionary<string, object>() {
                { "date" , DateTime.Now },
                { "int32" , 2 },
                { "int64" , 100000000 },
                { "fl" , 1.0 },
            };
            var inputParams =  string.Join("&", inputObj
                .Select(s =>s.Key+"=" + s.Value));
            //var resultContent = await client.PostAsync("api/services/WebApiTestModule/Normal/DefaultMethod", new StringContent(inputString, Encoding.UTF8,"application/json"));

            var request = new HttpRequestMessage(httpMethod, prePath + $"/WebApiTestModule/Normal/{methodName}Method?{inputParams}")
            {
               // Content = new StringContent(inputString, Encoding.UTF8, "application/json"),
            };
            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
            Assert.Equal(JsonConvert.SerializeObject(inputObj, timeFormat), resultContent);
        }

        private async Task TestSendDataAndReturn(string httpMethod, string methodName = null)
        {
            if (string.IsNullOrEmpty(methodName))
                methodName = httpMethod;
            var client = _factory.CreateClient();
            var inputObj = new {date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0};
            var inputString = JsonConvert.SerializeObject(inputObj);
            //var resultContent = await client.PostAsync("api/services/WebApiTestModule/Normal/DefaultMethod", new StringContent(inputString, Encoding.UTF8,"application/json"));

            var request = new HttpRequestMessage(new HttpMethod(httpMethod), prePath + $"/WebApiTestModule/Normal/{methodName}Method")
            {
                Content = new StringContent(inputString, Encoding.UTF8, "application/json")
            };
           
            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            Assert.Equal(inputString, resultContent);

        }
    }
}