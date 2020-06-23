using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BlocksCore.Test.Abstractions
{
    public class ServiceTestContextFix<TEntryPoint> : IDisposable where TEntryPoint : class
    {
       

        public WebApplicationFactory<TEntryPoint> Factory { get; set; }

        public  string PrePath = "/api/services";

        public JsonConvertWrapper JsonConvert { get; set; }
        public ServiceTestContextFix()
        {
            Factory = new CustomWebApplicationFactory<TEntryPoint>();
 
            JsonConvert = new JsonConvertWrapper();

        }
        public void Dispose()
        {
            Factory.Dispose();
        }
    }

    public class CustomWebApplicationFactory<TStartup>
     : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            return base.CreateHost(builder.UseServiceProviderFactory(new AutofacServiceProviderFactory()));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }
    }

    public class JsonConvertWrapper
    {
        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {

            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };
        public string SerializeObject(object obj, params JsonConverter[] converters)
        {
            if (converters == null)
                return JsonConvert.SerializeObject(obj, this.jsonSerializerSettings);
            JsonSerializerSettings jsonSerializerSettingsCombine = new JsonSerializerSettings
            {
                Converters = converters,
               // ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(obj, jsonSerializerSettingsCombine);
        }


        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

    }
}
