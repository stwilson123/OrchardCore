using System.Collections.Generic;
using BlocksCore.Abstractions;
using BlocksCore.Navigation.Abstractions;
using Microsoft.Extensions.Localization;

namespace BlocksCore.Test.Navigation.Model
{
    public class TestNavigationFileProvider : INavigationFileProvider
    {
        public IStringLocalizer T { get; set; }

        public IDictionary<Platform, string> filePaths => new Dictionary<Platform, string> {
            { Platform.Main, "Model/WebNavigation.json" }
        };

        //public void SetNavigation(INavigationProviderContext context)
        //{
        //    context.Manager.MainMenu.AddItem(new NavigationItemDefinition("Test", new LocalizedString("", "")));
        //    context.Manager.MainMenu.AddBuilder((builder) =>
        //        builder.Name("Test1").DisplayName(new LocalizedString("", "")).Action("abc", "controller", "TestNavigationModule")
        //    );
        //    context.Manager.MainMenu.AddBuilder((builder) =>
        //        builder.Name("Test2").DisplayName(new LocalizedString("", "")).Action("abc", "controller", "TestNavigationModule")
        //        );

        //}
    }
}
