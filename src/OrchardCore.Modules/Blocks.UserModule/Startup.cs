using System;
using System.Linq;
using BlocksCore.Navigation;
using BlocksCore.Navigation.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Security;
using BlocksCore.Users.Identity.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BlocksCore.Users.Abstractions;
using Microsoft.AspNetCore.Authorization;
using BlocksCore.Users.Services;

namespace Blocks.UserModule
{
    public class Startup : StartupBase
    {

        public Startup()
        {
            
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            ConfigureIdentity(services);
            services.AddSecurity();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

           
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseAuthorization();
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<IUser, IRole>((options) =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            });
            //.AddEntityFrameworkStores<ApplicationDbContext>();
            //   }

            services.TryAddScoped<UserStore>();
            services.TryAddScoped<IUserStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserRoleStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserPasswordStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserEmailStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserSecurityStampStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserLoginStore<IUser>>(sp => sp.GetRequiredService<UserStore>());
            services.TryAddScoped<IUserClaimStore<IUser>>(sp => sp.GetRequiredService<UserStore>());

            services.TryAddScoped<RoleManager<IRole>>();

            services.TryAddScoped<IRoleStore<IRole>, RoleStore>();
            services.TryAddScoped<IRoleClaimStore<IRole>, RoleStore>();
        }

    }
}
