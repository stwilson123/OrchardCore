using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Linq2DB.Test.TestModel;
using BlocksCore.Data.Linq2DB.Test.TestModel.BlockTestContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

        public override void Init(bool isDatabaseCreator = false)
        {
            Services.TryAddSingleton(isDatabaseCreator ? CreateDatabaseCreatorSettings() : CreateShellSettings());
            base.Init();
        }

        public override ShellSettings CreateShellSettings()
        {
            ShellSettings shellSettings = new ShellSettings();
            shellSettings["DatabaseProvider"] = DatabaseProviderName.Oracle;
            shellSettings["ConnectionString"] = ConnectionString;
            shellSettings["MasterConnectionString"] = masterConnectionString;
            return shellSettings;
        }

        public override ShellSettings CreateDatabaseCreatorSettings()
        {
            ShellSettings shellSettings = new ShellSettings();
            shellSettings["DatabaseProvider"] = DatabaseProviderName.Oracle;
            shellSettings["ConnectionString"] = ConnectionString.Replace("Min Pool Size=0","");
            shellSettings["MasterConnectionString"] = masterConnectionString + "  ";
            return shellSettings;
        }



    }
}
