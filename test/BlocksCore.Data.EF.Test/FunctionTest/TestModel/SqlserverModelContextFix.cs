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

        public override string ConnectionString { get; protected set; }

        public SqlserverModelContextFix(string connectionString) : base()
        {
            ConnectionString = connectionString;
           
        }

        public override void Init()
        {
            ShellSettings shellSettings = new ShellSettings();
            shellSettings["DatabaseProvider"] = ProviderName;
            shellSettings["ConnectionString"] = ConnectionString;
            Services.AddSingleton(shellSettings);
            base.Init();
        }
    }
}
