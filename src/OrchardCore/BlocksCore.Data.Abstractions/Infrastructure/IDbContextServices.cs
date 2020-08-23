using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BlocksCore.Data.Abstractions.Configurations;
using BlocksCore.Domain.Abstractions;

namespace BlocksCore.Data.Abstractions.Infrastructure
{
    public interface IDbContextServices
    {
        IDataContext CurrentContext { get; }


        IModel Model { get; }

        IServiceProvider InternalServiceProvider { get; }

        IDbContextServices Initialize(IServiceProvider scopedProvider,
            IDbContextOptions contextOptions,
            IDataContext context);

        ConnectionInfo GetConnetionInfo();

        IDbConnection CreateConnection(string connectionString);


    }
}
