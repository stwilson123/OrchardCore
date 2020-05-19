using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BlocksCore.Abstractions.Exception;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;

namespace OrchardCore.Environment.Extensions.Loaders
{
    public static class ExtensionEntryExtensions
    {
        public static IFileInfo GetFileInfo(this ExtensionEntry extensionEntry, string subpath)
        {
            if (extensionEntry == null || extensionEntry.Assembly == null)
                throw new BlocksException("ExtensionEntry or ExtensionEntry.Assembly is null.");


            var assembly = extensionEntry.Assembly;

            var resourcePath = assembly.GetName().Name + "." + subpath.Replace('/', '>');
            var fileName = Path.GetFileName(subpath);

            if (assembly.GetManifestResourceInfo(resourcePath) == null)
            {
                return new NotFoundFileInfo(fileName);
            }

            var fileProvider = new EmbeddedFileProvider(assembly);

            
            return fileProvider.GetFileInfo(resourcePath);
        }
    }
}
