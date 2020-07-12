using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BlocksCore.Data.Linq2DB.Test.TestConfiguration.Log
{
    public class LoggerProvider : ILoggerProvider
    {
        private List<string> LogList { set; get; }

        public LoggerProvider(List<string> LogList)
        {
            this.LogList = LogList;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new StringLogger(this.LogList);
        }

        public void Dispose()
        {
            
        }
    }
}
