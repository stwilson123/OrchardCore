using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using OrchardCore.Settings;

namespace BlocksCore.Settings
{
    public class SiteSettings : ISite
    {
        public string SiteName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string PageTitleFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SiteSalt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SuperUser { get => "Admin"; set => throw new NotImplementedException(); }
        public string Calendar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TimeZoneId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ResourceDebugMode ResourceDebugMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool UseCdn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CdnBaseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int PageSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int MaxPageSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int MaxPagedCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string BaseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RouteValueDictionary HomeRoute { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AppendVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public JObject Properties => throw new NotImplementedException();
    }
}
