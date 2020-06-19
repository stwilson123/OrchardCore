using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Extensions;
using OrchardCore.Localization;
using OrchardCore.Localization.PortableObject;

namespace BlocksCore.Localization.Core
{
    public class ModularPortableObjectStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ILocalizationManager _localizationManager;
        private readonly bool _fallBackToParentCulture;
        private readonly ILogger _logger;
        private readonly ITypeFeatureProvider _typeFeatureProvider;

        public ModularPortableObjectStringLocalizerFactory(ILocalizationManager localizationManager,
            IOptions<RequestLocalizationOptions> requestLocalizationOptions,
            ILogger<ModularPortableObjectStringLocalizerFactory> logger,
            ITypeFeatureProvider typeFeatureProvider)
        {
            _localizationManager = localizationManager;
            _fallBackToParentCulture = requestLocalizationOptions.Value.FallBackToParentUICultures;
            _logger = logger;
            this._typeFeatureProvider = typeFeatureProvider;
        }
        public IStringLocalizer Create(string baseName, string location)
        {
            var index = 0;
            if (baseName.StartsWith(location, StringComparison.OrdinalIgnoreCase))
            {
                index = location.Length;
            }

            if (baseName.Length > index && baseName[index] == '.')
            {
                index += 1;
            }

            if (baseName.Length > index && baseName.IndexOf("Areas.", index) == index)
            {
                index += "Areas.".Length;
            }

            var relativeName = baseName.Substring(index);

            return new PortableObjectStringLocalizer(relativeName, _localizationManager, _fallBackToParentCulture, _logger);

        }

        public IStringLocalizer Create(Type resourceSource)
        {
            var context = string.Empty;
            //TODO special type
            try
            {
                context = _typeFeatureProvider.GetFeatureForDependency(resourceSource)?.Name;
            }
            catch
            {

            }
            
            return new PortableObjectStringLocalizer(context, _localizationManager, _fallBackToParentCulture, _logger);
        }
    }
}
