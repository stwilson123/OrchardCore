using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using OrchardCore.Settings;

namespace BlocksCore.Settings.Services
{
    public class SiteService : ISiteService
    {
        public IChangeToken ChangeToken => throw new NotImplementedException();

        public Task<ISite> GetSiteSettingsAsync()
        {
            return Task.FromResult<ISite>(new SiteSettings());
        }

        public Task UpdateSiteSettingsAsync(ISite site)
        {
            throw new NotImplementedException();
        }
    }
}
