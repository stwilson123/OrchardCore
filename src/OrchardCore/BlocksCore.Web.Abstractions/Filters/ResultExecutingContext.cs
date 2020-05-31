using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Web.Abstractions.Filters
{
    public class ResultExecutingContext
    {
        public ResultExecutingContext(object controller)
        {
            Controller = controller;
        }

        public virtual bool Cancel { get; set; }
  
        public virtual object Controller { get;  }
 
        public virtual object Result { get; set; }
    }
}
