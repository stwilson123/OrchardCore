using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
namespace BlocksCore.Abstractions.Extensions
{
    public static class FileInfoExtensions
    {
        public static async Task<string> ReadToString(this IFileInfo fileInfo)
        {

            if (fileInfo?.Exists ?? false)
            {
                using (var reader = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(reader))
                    {
                       
                        return await sr.ReadToEndAsync();
                    }
                }
            }
            return (string)null;
        }
    }
}
