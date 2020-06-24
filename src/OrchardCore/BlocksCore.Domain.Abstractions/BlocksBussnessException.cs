using System;
using System.Collections.Generic;
using System.Text;
using BlocksCore.Abstractions.Exception;
using Microsoft.Extensions.Localization;

namespace BlocksCore.Domain.Abstractions
{
    public class BlocksBussnessException : BlocksException
    {
        public BlocksBussnessException(string code, LocalizedString message) : base(code, message)
        {
        }

        public BlocksBussnessException(string code, LocalizedString message, object content) : base(code, message, content)
        {
        }
    }
}
