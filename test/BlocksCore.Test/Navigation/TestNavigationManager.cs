using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlocksCore.Abstractions;
using BlocksCore.Navigation.Abstractions;
using BlocksCore.Navigation.Core;
using BlocksCore.Test.Application.Controller.TestModel;
using BlocksCore.Test.Navigation.Model;
using BlocksCore.Test.Stubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Navigation;
using OrchardCore.Tests.Stubs;
using Xunit;

namespace BlocksCore.Test.Navigation
{
    public class TestNavigationManager
    {
        private IServiceProvider serviceProvider;
        public TestNavigationManager()
        {

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<INavigationFileManager, NavigationFileManager>();
            serviceCollection.AddTransient<INavigationFileProvider, TestNavigationFileProvider>();
            serviceCollection.AddTransient<ITypeFeatureProvider, DefaultTypeFeatureProvider>();
            serviceCollection.AddSingleton<IHostEnvironment>(new StubHostingEnvironment() { ContentRootPath = Directory.GetCurrentDirectory() + "../../../../" });
            serviceCollection.AddSingleton<INavigationManager, NavigationManager>();
            serviceCollection.AddSingleton<ILogger<NavigationManager>, NullLogger<NavigationManager>>();
            serviceCollection.AddSingleton<IStringLocalizer<AutoRegisterNavigationProvider>, NullStringLocalizer<AutoRegisterNavigationProvider>>();
            serviceCollection.AddSingleton<IStringLocalizerFactory , NullStringLocalizerFactory>();


            serviceCollection.AddSingleton<ShellSettings>(new ShellSettings());

            serviceCollection.AddSingleton<IUrlHelperFactory, NullUrlHelperFactory>();
            serviceCollection.AddSingleton<IAuthorizationService, NullAuthorizationService>();

            serviceCollection.AddSingleton<INavigationProvider, AutoRegisterNavigationProvider>();

            serviceProvider = serviceCollection.BuildServiceProvider();

        }

        [Fact]
        public async void NavigationProvider_Return_Suitable_Data()
        {
            var navigationManager = serviceProvider.GetService<INavigationManager>();


            var mainMenus = await navigationManager.GetFilterMenuAsync(Platform.Main.ToString(), new Microsoft.AspNetCore.Mvc.ActionContext(new StubHttpContext(), new RouteData(), new ActionDescriptor()));



            var factoryWeb = mainMenus.SingleOrDefault(i => i.Name == "FactoryWeb");
            var workcenterWeb = mainMenus.SingleOrDefault(i => i.Name == "WorkcenterWeb");

            Assert.NotNull(factoryWeb);
            Assert.NotNull(workcenterWeb);

            Assert.True(mainMenus.Count(i => RouteValuesEquals(i.RouteValues, new RouteValueDictionary()
            {
                { "area" ,"TestModule"},
                { "controller" ,"Workcenter"},
                { "action" ,"Index"},
            })) == 1);


            Assert.True(workcenterWeb.Permissions.Count(p => p.Name == "TestModule/WorkcenterIndex/Add") == 1);
            Assert.True(workcenterWeb.Permissions.Count(p => p.Name == "TestModule/WorkcenterIndex/Index") == 1);

        }

        private static bool RouteValuesEquals(IDictionary<string, object> dic, IDictionary<string, object> other)
        {
            if (dic == null)
                return false;

            if (dic.Count != other.Count)
                return false;

            foreach (var kv in dic)
            {
                if (!kv.Value.Equals(other[kv.Key]))
                    return false;
            }

            return true;
        }


    }



    class NullUrlHelperFactory : IUrlHelperFactory
    {
        public IUrlHelper GetUrlHelper(ActionContext context)
        {
            return new NullUrlHelper();
        }
    }

    class NullUrlHelper : IUrlHelper
    {
        public ActionContext ActionContext => throw new NotImplementedException();

        public string Action(UrlActionContext actionContext)
        {
            throw new NotImplementedException();
        }

        public string Content(string contentPath)
        {
            throw new NotImplementedException();
        }

        public bool IsLocalUrl(string url)
        {
            throw new NotImplementedException();
        }

        public string Link(string routeName, object values)
        {
            throw new NotImplementedException();
        }

        public string RouteUrl(UrlRouteContext routeContext)
        {
            return "";
        }
    }

    class NullAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(AuthorizationResult.Success());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            throw new NotImplementedException();
        }
    }


}
