using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Core.Configurations;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace BlocksCore.Data.Linq2DB.SQLServer
{
    public class SQLServerDbContextOptionBuilder : DbContextOptionBuilder<LinqToDbConnectionOptions>
    {
        private readonly IDbContextOptionBuilder<LinqToDbConnectionOptions> dbContextOptionBuilder;
        private readonly DbConnection dbConnection;

        public SQLServerDbContextOptionBuilder(IDbContextOptionBuilder<LinqToDbConnectionOptions> dbContextOptionBuilder,DbConnection dbConnection)
        {
            this.dbContextOptionBuilder = dbContextOptionBuilder;
            this.dbConnection = dbConnection;
        }

        public override DbContextOption<LinqToDbConnectionOptions> Build()
        {
            var linq2dbBuilder = new LinqToDbConnectionOptionsBuilder();
            var dbProvider = Linq2DBMap.GetDataProvider("Microsoft.Data.SqlClient", dbConnection.ConnectionString);
            linq2dbBuilder.UseConnection(dbProvider, dbConnection);
            var that = dbContextOptionBuilder.WithOption(linq2dbBuilder.Build())
            .AddOrUpdateExtension(new SQLServerDbContextOptionExtensions()).Build();
            return that;
        }
    }
}
