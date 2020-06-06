using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Exception;

namespace BlocksCore.Data.Abstractions
{
    public class BlocksDataException : BlocksException
    {
        static string ExceptionCode = "1001";
        public override string Code { get; protected set; } = ExceptionCode;
        public BlocksDataException(string message) : this(message,null)
        {
        }

        public BlocksDataException(string message, Exception innerException) : base(ExceptionCode, message, innerException)
        {
        }
    }
}
