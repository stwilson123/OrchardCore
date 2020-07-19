using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BlocksCore.Data.Migrator
{
    public class DebuggerTextWriter : StringWriter
    {
        public override void Flush()
        {
            var sb = this.GetStringBuilder();

            Debug.Write(sb.ToString());
            base.Flush();
        }
    }
}
