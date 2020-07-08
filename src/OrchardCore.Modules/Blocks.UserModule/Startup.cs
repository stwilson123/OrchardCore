using System;
using System.Linq;
using BlocksCore.Navigation;
using BlocksCore.Navigation.Abstractions;
using BlocksCore.Security;
using BlocksCore.Users.Abstractions;
using BlocksCore.Users.Identity.Store;
using BlocksCore.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Environment.Extensions;
using OrchardCore.Modules;
 
using OrchardCore.Navigation;
using OrchardCore.Security;

namespace Blocks.UserModule
{
    public class Startup : StartupBase
    {

        public Startup()
        {
            
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSecurity();
            services.AddSecurityCore();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            ConfigureIdentity(services);
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseAuthorization();
        }

        void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<IUser, IRole>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            });

            services.TryAddScoped<UserStore>();
            services.TryAddScoped<IUserStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            // services.TryAddScoped<IUserRoleStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserPasswordStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserEmailStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserSecurityStampStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserLoginStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            //  services.TryAddScoped<IUserClaimStore<IUser>>(sp => sp.GetRequiredService<UserStore>());

            services.TryAddScoped<RoleManager<IRole>>();

            services.TryAddScoped<IRoleStore<IRole>, RoleStore>();
            services.TryAddScoped<IRoleClaimStore<IRole>, RoleStore>();

            //  services.TryAddScoped<RoleManager<IRole>>();
        }


    }
}
