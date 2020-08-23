using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BlocksCore.Data.Linq2DB.Test.TestConfiguration.Log
{
    public class StringLogger : ILogger
    {
        public List<string> LogList { set; get; }
        public StringLogger(List<string> LogList)
        {
            this.LogList = LogList;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

            LogList.Add(formatter(state, exception));
        }
    }
}
