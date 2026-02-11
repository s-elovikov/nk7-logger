using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System;

namespace Nk7.Logger
{
    public interface ILoggerService
    {
        bool IsEnabled(LogLevel logLevel);

        void Debug([InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler);
        void Information([InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler);
        void Warning([InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler);
        void Error([InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler);

        void Debug(Exception exception, [InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler);
        void Information(Exception exception, [InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler);
        void Warning(Exception exception, [InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler);
        void Error(Exception exception, [InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler);
    }
}
