using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BlocksCore.Data;
using BlocksCore.Data.Linq2DB;
using BlocksCore.Domain.Abstractions;
using LinqToDB;
using LinqToDB.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrchardCoreBuilderExtensions
    {
        /// <summary>
        /// Adds tenant level data access services.
        /// </summary>
        public static OrchardCoreBuilder AddLinq2DBDataAccess(this OrchardCoreBuilder builder)
        {

            return builder.RegisterStartup<Startup>().ConfigureServices((services, serviceProvider) =>
            {
               
            });
        }
    }
}