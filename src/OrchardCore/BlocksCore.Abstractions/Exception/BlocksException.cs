using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Exception
{
    [Serializable]
    public class BlocksException : System.Exception
    {
        public virtual string Code { protected set; get; }
        public object Content { protected set; get; }
        public BlocksException(string message)
            : base(message)
        {

        }
        public BlocksException(string message, System.Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
