using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.DataBaseProvider;
using FluentMigrator.Runner;

namespace BlocksCore.Data.Migrator
{
    public static class DataBaseSelect
    {
        public static IMigrationRunnerBuilder AddDataBaseProvider(this IMigrationRunnerBuilder builder, ProviderName databaseProviderName)
        {
            switch (databaseProviderName)
            {
                case ProviderName.SqlServer2005:builder.AddSqlServer2005();break;
                case ProviderName.SqlServer2008: builder.AddSqlServer2008(); break;
                case ProviderName.SqlServer2012: builder.AddSqlServer2012(); break;
                case ProviderName.SqlServer2014: builder.AddSqlServer2014(); break;
                case ProviderName.SqlServer2017: builder.AddSqlServer2016(); break;
                case ProviderName.OracleManaged: builder.AddOracleManaged(); break;

                case ProviderName.MySql: builder.AddMySql5(); break;
               
                default: builder.AddSqlServer2008(); break;
            }

            return builder;

        }
    }
}
