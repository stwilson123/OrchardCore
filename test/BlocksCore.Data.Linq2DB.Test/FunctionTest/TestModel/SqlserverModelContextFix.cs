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
        private readonly string masterConnectionString;

        public override string ProviderName { get; } = DatabaseProviderName.Sqlserver;

        public override string ConnectionString { get; protected set; }

        public SqlserverModelContextFix(string connectionString, string masterConnectionString) : base(connectionString)
        {
            this.masterConnectionString = masterConnectionString;
        }

        public override void Init(bool isDatabaseCreator = false)
        {
            Services.AddSingleton(CreateShellSettings());
            base.Init();
        }

        public override ShellSettings CreateShellSettings()
        {
            ShellSettings shellSettings = new ShellSettings();
            shellSettings["DatabaseProvider"] = ProviderName;
            shellSettings["ConnectionString"] = ConnectionString;
            shellSettings["MasterConnectionString"] = masterConnectionString;

            return shellSettings;
        }

        public override ShellSettings CreateDatabaseCreatorSettings()
        {
            ShellSettings shellSettings = new ShellSettings();
            shellSettings["DatabaseProvider"] = ProviderName;
            shellSettings["ConnectionString"] = masterConnectionString + "  ";
            shellSettings["MasterConnectionString"] = masterConnectionString + "  ";

            return shellSettings;
        }
    }
}
