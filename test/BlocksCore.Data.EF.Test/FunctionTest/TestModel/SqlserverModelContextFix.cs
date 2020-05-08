using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.EF.Test.TestModel;
using BlocksCore.Data.EF.Test.TestModel.BlockTestContext;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.EF.Test.FunctionTest.TestModel
{
    public class SqlserverModelContextFix : TestModelContext
    {
        public override string ProviderName { get; } = DatabaseProviderName.Sqlserver;

        public override string ConnectionString { get; } = String.Format(ConfigurationHelper.Config[TestBlocksDbContext.SqlserverConnectString], Guid.NewGuid().ToString("N"));

        public SqlserverModelContextFix() : base()
        {

           
        }

        public override void Init(IServiceCollection services)
        {
            ShellSettings shellSettings = new ShellSettings();
            shellSettings["DatabaseProvider"] = ProviderName;
            shellSettings["ConnectionString"] = ConnectionString;
            Services.AddSingleton(shellSettings);
        }
    }
}
