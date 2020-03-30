using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BlocksCore.Diagnostics
{
    public class DiagnosticsStartupFilter : IStartupFilter
    {

        private readonly IHostEnvironment _hostEnvironment;

        public DiagnosticsStartupFilter(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                if (!_hostEnvironment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                }

               // app.UseStatusCodePagesWithReExecute("/Error/{0}");

                app.Use(async (context, next) =>
                {
                    await next();

                    if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 400)
                    {
                        //if (_contentTypeProvider.TryGetContentType(context.Request.Path, out var contentType))
                        //{
                        //    var statusCodePagesFeature = context.Features.Get<IStatusCodePagesFeature>();
                        //    if (statusCodePagesFeature != null)
                        //    {
                        //        statusCodePagesFeature.Enabled = false;
                        //    }
                        //}
                    }
                });

                next(app);
            };
        }
    }
}
