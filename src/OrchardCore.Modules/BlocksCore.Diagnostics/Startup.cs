using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrchardCore.Modules;

namespace BlocksCore.Diagnostics
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<Microsoft.AspNetCore.Hosting.IStartupFilter, DiagnosticsStartupFilter>());
            System.Diagnostics.DiagnosticListener.AllListeners.Subscribe(new CommandListener());
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
               name: "Diagnostics.Error",
               areaName: "BlocksCore.Diagnostics",
               pattern: "Error/{status?}",
               defaults: new { controller = "Diagnostics", action = "Error" }
           );
        }
    }

    public class CommandListener : IObserver<System.Diagnostics.DiagnosticListener>
    {
        private readonly DbCommandInterceptor _dbCommandInterceptor = new DbCommandInterceptor();

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(System.Diagnostics.DiagnosticListener listener)
        {

            if (listener.Name == "Microsoft.AspNetCore")
            {
                listener.Subscribe(_dbCommandInterceptor);
            }

//            if (listener.Name == DbLoggerCategory.Name)
//            {
//                listener.Subscribe(_dbCommandInterceptor);
////                listener.Subscribe(_dbCommandInterceptor,eventName => eventName == RelationalEventId.CommandExecuting.Name);
////                listener.Subscribe(_dbCommandInterceptor,eventName => eventName == RelationalEventId.CommandExecuted.Name);
//
//
//            }
        }
    }


    public class DbCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted()
        {
           // throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
           // throw new NotImplementedException();
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            //throw new NotImplementedException();
        }
    }
}
