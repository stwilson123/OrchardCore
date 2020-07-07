using BlocksCore.Domain.Abstractions;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysMgt.BussnessDomainModule.Common
{
    public class HelperBLL
    {
        public static void ThrowEx(string code4Err, LocalizedString stringLocal)
        {
            throw new BlocksBussnessException(code4Err, stringLocal, null);
        }
    }
}
