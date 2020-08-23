using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Data.Abstractions.DataBaseProvider;
using BlocksCore.Data.Abstractions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Abstractions.Configurations
{
    public interface IDbContextOptionExtensions
    {
        public bool ApplyServices(IServiceCollection services,IServiceProvider serviceProvider,ConnectionInfo connectInfo);
 
    }
}
