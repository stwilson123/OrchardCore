using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
namespace BlocksCore.Abstractions.extensions
{
    public static class FileInfoExtensions
    {
        public static Task<string> ReadToString(this IFileInfo fileInfo)
        {

            if (fileInfo?.Exists ?? false)
            {
                using (var reader = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(reader))
                    {
                        return sr.ReadToEndAsync()
                    }
                }
            }
            return Task.FromResult<string>(null);
        }
    }
}
