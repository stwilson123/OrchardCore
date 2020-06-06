using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ExceptionContext : FilterContext
    {
        public ExceptionContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public   Exception Exception { get; set; }

        public   bool ExceptionHandled { get; set; }
        public   object Result { get; set; }
    }
}
