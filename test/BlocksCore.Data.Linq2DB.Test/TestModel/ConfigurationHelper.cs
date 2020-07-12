using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BlocksCore.Data.Linq2DB.Test.TestModel
{
    public class ConfigurationHelper
    {
        static ConfigurationHelper()
        {
            Config = InitConfiguration();
        }
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

        public static IConfiguration Config { get; }
    }
}
