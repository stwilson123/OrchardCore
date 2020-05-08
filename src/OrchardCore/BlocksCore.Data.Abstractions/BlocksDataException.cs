using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Exception;

namespace BlocksCore.Data.Abstractions
{
    public class BlocksDataException : BlocksException
    {
        public override string Code { get; protected set; } = "1001";
        public BlocksDataException(string message) : base(message)
        {
        }

        public BlocksDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
