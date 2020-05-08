using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BlocksCore.Data.EF.Logging
{
    public class DbCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {

            //Trace.TraceError("Exception:{1} \r\n --> Error executing command:\r\n {0}", command.CommandText, interceptionContext.Exception.ToString());

        }

        public void OnNext(KeyValuePair<string, object> value)
        { 
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                _stopwatch.Restart();
               
            }

            if(value.Key == RelationalEventId.CommandExecuted.Name)
            {
                _stopwatch.Stop();
                var command= ((CommandEventData)value.Value).Command;

                
               
                Trace.TraceInformation("\r\n执行时间:{0} 毫秒 \r\n -->CommandExecuted.Command:\r\n{1}\r\nParamter:{2}", _stopwatch.ElapsedMilliseconds, command.CommandText,
                    string.Join(",",command.Parameters.Cast<IDbDataParameter>().Select(t => string.Format("{0}:{1};" ,t.ParameterName,t.Value)))
                    );
                //var command = ((CommandEventData)value.Value).Command;
                //var executeMethod = ((CommandEventData)value.Value).ExecuteMethod;

                //if (executeMethod == DbCommandMethod.ExecuteNonQuery)
                //{

                //}
                //else if (executeMethod == DbCommandMethod.ExecuteScalar)
                //{

                //}
                //else if (executeMethod == DbCommandMethod.ExecuteReader)
                //{

                //}
            }

            if(value.Key == RelationalEventId.CommandError.Name)
            {
                _stopwatch.Stop();
                var command = ((CommandEventData)value.Value).Command;
                Trace.TraceInformation("\r\n执行时间:{0} 毫秒 \r\n -->CommandError.Command:\r\n{1}\r\nParamter:{2}", _stopwatch.ElapsedMilliseconds, command.CommandText,
                    string.Join(",",command.Parameters.Cast<IDbDataParameter>().Select(t => string.Format("{0}:{1};" ,t.ParameterName,t.Value)))
                    );
            }


            if(value.Key == "Microsoft.Data.SqlClient.WriteCommandBefore")
            {
                //var command = ((CommandEventData)value.Value).Command;
                //Trace.TraceInformation("\r\n执行时间:{0} 毫秒 \r\n -->CommandError.Command:\r\n{1}\r\nParamter:{2}", _stopwatch.ElapsedMilliseconds, command.CommandText,
                //    string.Join(",", command.Parameters.Cast<IDbDataParameter>().Select(t => string.Format("{0}:{1};", t.ParameterName, t.Value)))
                //    );
            }
        }

       
    }
}
