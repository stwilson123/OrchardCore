using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Test.Abstractions;
using BlocksCore.Web.Abstractions.Result;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApiTestApplication;
using Xunit;

namespace BlocksCore.WebApi.Test
{
    public class AppServiceTest : IClassFixture<ServiceTestContextFix<Startup>>
    {
        
        private readonly ServiceTestContextFix<Startup> serviceTestContextFix;

        public AppServiceTest(ServiceTestContextFix<Startup> serviceTestContextFix)
        {
            this.serviceTestContextFix = serviceTestContextFix;
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
            var client = this.serviceTestContextFix.Factory.CreateClient();
            var result = await client.GetAsync(serviceTestContextFix.PrePath + "/WebApiTestModule/Normal/SpecialMethod");
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);

            result = await client.PostAsync(serviceTestContextFix.PrePath+"/WebApiTestModule/Normal/GetMethod",null);
            resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, result.StatusCode);
        }


        [Fact]
        public async void TestDefaultModeMethodCanUseAllHttpMethod()
        {
            var client = this.serviceTestContextFix.Factory.CreateClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, serviceTestContextFix.PrePath+"/WebApiTestModule/Normal/DefaultMethod") {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
           
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.SendAsync(httpRequest);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
 
            result = await client.PostAsync(serviceTestContextFix.PrePath + "/WebApiTestModule/Normal/DefaultMethod", new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            result = await client.PutAsync(serviceTestContextFix.PrePath + "/WebApiTestModule/Normal/DefaultMethod",   new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
         
        }

        private async Task TestSendSimpleData(HttpMethod httpMethod, string methodName = null)
        {
            if (string.IsNullOrEmpty(methodName))
                methodName = httpMethod.Method;
            var client = this.serviceTestContextFix.Factory.CreateClient();
            var inputObj = new Dictionary<string, object>() {
                { "date" , DateTime.Now },
                { "int32" , 2 },
                { "int64" , 100000000 },
                { "fl" , 1.0 },
            };
            var inputParams =  string.Join("&", inputObj
                .Select(s =>s.Key+"=" + s.Value));
            //var resultContent = await client.PostAsync("api/services/WebApiTestModule/Normal/DefaultMethod", new StringContent(inputString, Encoding.UTF8,"application/json"));

            var request = new HttpRequestMessage(httpMethod, serviceTestContextFix.PrePath + $"/WebApiTestModule/Normal/{methodName}Method?{inputParams}")
            {
               // Content = new StringContent(inputString, Encoding.UTF8, "application/json"),
            };
            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
            var expectValue = this.serviceTestContextFix.JsonConvert.SerializeObject(new DataResult() { Code = "200",Content = inputObj }, timeFormat);
            Assert.Equal(expectValue, resultContent);
        }

        private async Task TestSendDataAndReturn(string httpMethod, string methodName = null)
        {
            if (string.IsNullOrEmpty(methodName))
                methodName = httpMethod;
            var client = this.serviceTestContextFix.Factory.CreateClient();
            var inputObj = new {date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0};
            var inputString = JsonConvert.SerializeObject(inputObj);
            //var resultContent = await client.PostAsync("api/services/WebApiTestModule/Normal/DefaultMethod", new StringContent(inputString, Encoding.UTF8,"application/json"));

            var request = new HttpRequestMessage(new HttpMethod(httpMethod), serviceTestContextFix.PrePath + $"/WebApiTestModule/Normal/{methodName}Method")
            {
                Content = new StringContent(inputString, Encoding.UTF8, "application/json")
            };
           
            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);


            var expectValue = this.serviceTestContextFix.JsonConvert.SerializeObject(new DataResult() { Code = "200", Content = inputObj });

            Assert.Equal(expectValue, resultContent);

        }
    }
}