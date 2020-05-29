using System;
using System.Linq;
using System.Text;
using BlocksCore.Users.Abstractions;
using BlocksCore.Web.Abstractions.Filters;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using OrchardCore.Environment.Extensions;
using OrchardCore.Modules;
using OrchardCore.Security;

namespace BlocksCore.JWTAuthentication
{
    public class Startup : StartupBase
    {
        private readonly IExtensionManager _extensionManager;
        private readonly IConfiguration configuration;

        public override int Order => base.Order + 10;

        public Startup(IExtensionManager extensionManager,IConfiguration configuration)
        {
            _extensionManager = extensionManager;
            this.configuration = configuration;
            //  this._shellDescriptor = shellDescriptor;
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IActionFilter, AppSerivceActionFilter>();
            ConfigureJWT(services);
            services.AddTransient<TokenService>();
        }

       

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            
        }
     
        void ConfigureJWT(IServiceCollection services)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSettings();
            configuration.Bind("JwtSettings", jwtSettings);

            //添加身份验证
            services.AddAuthentication(options =>
            {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //jwt token参数设置
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,
                    //颁发给谁
                    ValidAudience = jwtSettings.Audience,
                    //这里的key要进行加密
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                    /***********************************TokenValidationParameters的参数默认值***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // 是否要求Token的Claims中必须包含Expires
                    // RequireExpirationTime = true,
                    // 允许的服务器时间偏移量
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    // ValidateLifetime = true
                };
            });
        }

    }
}
