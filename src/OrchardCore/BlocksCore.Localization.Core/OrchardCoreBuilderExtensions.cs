using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;



namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrchardCoreBuilderExtensions
    {
        /// <summary>
        /// Adds tenant level data access services.
        /// </summary>
        public static OrchardCoreBuilder AddLocalization(this OrchardCoreBuilder builder)
        {
            return builder.ConfigureServices(services => services.AddLocalizationCore());
        }
    }
}