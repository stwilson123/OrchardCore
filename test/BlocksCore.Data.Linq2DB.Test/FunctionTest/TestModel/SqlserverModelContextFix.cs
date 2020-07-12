using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Linq2DB.Test.TestModel;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel
{
    public class SqlserverModelContextFix : TestModelContext
    {
        public override string ProviderName { get; } = DatabaseProviderName.Sqlserver;

        public override string ConnectionString { get; protected set; }

        public SqlserverModelContextFix(string connectionString) : base(connectionString)
        {
           
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
