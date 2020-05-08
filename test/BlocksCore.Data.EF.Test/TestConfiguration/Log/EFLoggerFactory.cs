using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlocksCore.Data.EF.Test.TestConfiguration.Log
{
    public class EFLoggerFactory
    {
        public static readonly Func<List<string>, ILoggerFactory> CreateLoggerFactory = (List<string> LogList) => LoggerFactory.Create(builder =>
        {
            builder
               .AddFilter((category, level) =>
                   category == DbLoggerCategory.Database.Command.Name)
               .AddProvider(new LoggerProvider(LogList));
        });


    }
}
