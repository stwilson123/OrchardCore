using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ResultExecutedContext
    {
        public ResultExecutedContext(object controller, object result)
        {
            Controller = controller;
            Result = result;
        }

        public virtual bool Canceled { get; set; }
        
        public virtual object Controller { get; }
        
        public virtual Exception Exception { get; set; }
       
        
        public virtual bool ExceptionHandled { get; set; }
       
        public virtual object Result { get; }
    }
}
