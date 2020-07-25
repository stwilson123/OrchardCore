using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Linq2DB.Test.TestModel;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;

namespace BlocksCore.Data.Linq2DB.Test.FunctionTest.TestModel
{
    public class OracleModelContextFix : TestModelContext
    {
        private readonly string masterConnectionString;

        public override string ProviderName { get; } = DatabaseProviderName.Oracle;

        public override string ConnectionString { get; protected set; }

        public OracleModelContextFix(string connectionString,string masterConnectionString) : base(connectionString)
        {
            this.masterConnectionString = masterConnectionString;
        }

        public override void Init()
        {
            ShellSettings shellSettings = new ShellSettings();
            shellSettings["DatabaseProvider"] = ProviderName;
            shellSettings["ConnectionString"] = ConnectionString;
            shellSettings["MasterConnectionString"] = masterConnectionString;

            
            Services.AddSingleton(shellSettings);
            base.Init();
        }
    }
}
