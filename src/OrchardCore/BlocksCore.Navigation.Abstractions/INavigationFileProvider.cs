using System;
using System.Collections;
using System.Collections.Generic;
using BlocksCore.Abstractions;

namespace BlocksCore.Navigation.Abstractions
{
    public interface INavigationFileProvider
    {
        IDictionary<Platform, string> filePaths { get; }
    }
}
