using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WebApiTestApplication;
using Xunit;

namespace BlocksCore.WebApi.Test
{
    public class ControllerTest
    {
        private WebApplicationFactory<Startup> _factory;

        public ControllerTest()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [Fact]
        public async void Test1()
        {
            var client = _factory.CreateClient();
            var inputObj = new {date = DateTime.Now, int32 = 2, int64 = 100000000, fl = 1.0};
            var inputString = JsonConvert.SerializeObject(inputObj);
            var resultContent = await client.PostAsync("/WebApiTestModule/Admin/DefaultMethodFromObject", new StringContent(inputString, Encoding.UTF8,"application/json"));

            var result = await resultContent.Content.ReadAsStringAsync();

            Assert.Equal(inputString,result);

        }
    }
}