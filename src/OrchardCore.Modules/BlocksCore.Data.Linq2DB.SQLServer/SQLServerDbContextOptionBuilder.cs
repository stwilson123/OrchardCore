using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Data.Core.Configurations;
using BlocksCore.Domain.Abstractions;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace BlocksCore.Data.Linq2DB.SQLServer
{
    public class SQLServerDbContextOptionBuilder : DbContextOptionBuilder<LinqToDbConnectionOptions>
    {
        private readonly IDbContextOptionBuilder<LinqToDbConnectionOptions> dbContextOptionBuilder;
        private readonly IUnitOfWork _unitOfWork;

        public SQLServerDbContextOptionBuilder(IDbContextOptionBuilder<LinqToDbConnectionOptions> dbContextOptionBuilder, IUnitOfWork unitOfWork)
        {
            this.dbContextOptionBuilder = dbContextOptionBuilder;
            this._unitOfWork = unitOfWork;
        }

        public override DbContextOption<LinqToDbConnectionOptions> Build()
        {
            var linq2dbBuilder = new LinqToDbConnectionOptionsBuilder();

            var connection = _unitOfWork.DbConnection;
            var dbProvider = Linq2DBMap.GetDataProvider("Microsoft.Data.SqlClient", connection.ConnectionString);
            if (_unitOfWork.DbTransaction != null)
                linq2dbBuilder.UseTransaction(dbProvider, _unitOfWork.DbTransaction);
            else
                linq2dbBuilder.UseConnection(dbProvider, connection);

            var that = dbContextOptionBuilder.WithOption(linq2dbBuilder.Build())
            .AddOrUpdateExtension(new SQLServerDbContextOptionExtensions()).Build();
            return that;
        }
    }
}
