using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlocksCore.Data.EF.Logging
{
    public class CommandListener : IObserver<System.Diagnostics.DiagnosticListener>
    {
        private readonly DbCommandInterceptor _dbCommandInterceptor = new DbCommandInterceptor();

        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(System.Diagnostics.DiagnosticListener listener)
        {
            
            if (listener.Name == DbLoggerCategory.Name)
            {
                listener.Subscribe(_dbCommandInterceptor);
//                listener.Subscribe(_dbCommandInterceptor,eventName => eventName == RelationalEventId.CommandExecuting.Name);
//                listener.Subscribe(_dbCommandInterceptor,eventName => eventName == RelationalEventId.CommandExecuted.Name);

                
            }
           
            if (listener.Name == "SqlClientDiagnosticListener")
            {
                listener.Subscribe(_dbCommandInterceptor);
            }
        }
    }
}
