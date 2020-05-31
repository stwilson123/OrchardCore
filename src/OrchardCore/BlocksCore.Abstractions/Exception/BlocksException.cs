using System;
using System.Collections.Generic;
using System.Text;

namespace BlocksCore.Abstractions.Exception
{
    [Serializable]
    public class BlocksException : System.Exception
    {
        public  virtual string Code { protected set; get; }
        public object Content { protected set; get; }
    

        public BlocksException(string code, string message)
            : this(code,message,null,null)
        {

        }
        public BlocksException(string code, string message, object content)
           : this(code, message, content,null)
        {

        }

        public BlocksException(string code, string message, System.Exception innerException)
            : this(code, message, null, innerException)
        {
           
        }
        public BlocksException(string code, string message, object content, System.Exception innerException)
          : base(message, innerException)
        {
            this.Code = code;
            this.Content = content;
        }

    }
}
