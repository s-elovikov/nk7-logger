using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System;

namespace Nk7.Logger
{
    public interface ILoggerService
    {
        bool IsEnabled(LogLevel logLevel);

        void Trace(string message);
        void Debug(string message);
        void Information(string message);
        void Warning(string message);
        void Error(string message);
        void Critical(string message);

        public void Trace(Exception exception, string message);
        public void Debug(Exception exception, string message);
        public void Information(Exception exception, string message);
        public void Warning(Exception exception, string message);
        public void Error(Exception exception, string message);
        public void Critical(Exception exception, string message);

        void Trace([InterpolatedStringHandlerArgument("")] ref TraceLogInterpolatedStringHandler handler);
        void Debug([InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler);
        void Information([InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler);
        void Warning([InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler);
        void Error([InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler);
        void Critical([InterpolatedStringHandlerArgument("")] ref CriticalLogInterpolatedStringHandler handler);

        void Trace(Exception exception, [InterpolatedStringHandlerArgument("")] ref TraceLogInterpolatedStringHandler handler);
        void Debug(Exception exception, [InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler);
        void Information(Exception exception, [InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler);
        void Warning(Exception exception, [InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler);
        void Error(Exception exception, [InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler);
        void Critical(Exception exception, [InterpolatedStringHandlerArgument("")] ref CriticalLogInterpolatedStringHandler handler);
    }
}
