using BlocksCore.Navigation.Abstractions;
using BlocksCore.Navigation.Abstractions.Provider;
using Microsoft.Extensions.Localization;

namespace BlocksCore.Test.Navigation.Model
{
    public class TestNavigationProvider : INavigationProvider
    {
        public IStringLocalizer T { get; set; }

        public void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu.AddItem(new NavigationItemDefinition("Test", new LocalizedString("", "")));
            context.Manager.MainMenu.AddBuilder((builder) =>
                builder.Name("Test1").DisplayName(new LocalizedString("", "")).Action("abc", "controller", "TestNavigationModule")
            );
            context.Manager.MainMenu.AddBuilder((builder) =>
                builder.Name("Test2").DisplayName(new LocalizedString("", "")).Action("abc", "controller", "TestNavigationModule")
                );

        }
    }
}
