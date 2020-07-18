using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace BlocksCore.Data.Abstractions.Configurations
{
    public interface IDbContextOptionBuilder<TOption> 
    {
        IDbContextOptionBuilder<TOption> AddOrUpdateExtension(IDbContextOptionExtensions extensions);

        IDbContextOptionBuilder<TOption> WithOption(TOption option);

        DbContextOption<TOption> Build();
    }
}
