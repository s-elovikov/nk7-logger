using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System;

namespace Nk7.Logger
{
    public sealed class EmptyLoggerService : ILoggerService
    {
        public bool IsEnabled(LogLevel logLevel) => false;

        public void Debug([InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler) { }

        public void Information([InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler) { }

        public void Warning([InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler) { }

        public void Error([InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler) { }

        public void Debug(Exception exception, [InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler) { }

        public void Information(Exception exception, [InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler) { }

        public void Warning(Exception exception, [InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler) { }

        public void Error(Exception exception, [InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler) { }
    }
}
