using System;
using BlocksCore.Web.Abstractions.Filters;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BlocksCore.Data.Transaction
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActionFilter, TransactionActionFilter>();
            services.AddSingleton<IExceptionFilter, TransactionExceptionFilter>();

        }
    }
}
