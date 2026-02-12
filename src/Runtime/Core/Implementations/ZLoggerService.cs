using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System;

namespace Nk7.Logger
{
    public sealed class ZLoggerService : ILoggerService
    {
        private readonly ILogger _logger;

        public ZLoggerService(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Debug(string message)
        {
            if (!IsEnabled(LogLevel.Debug))
            {
                return;
            }

            _logger.LogDebug(message);
        }

        public void Information(string message)
        {
            if (!IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(message);
        }

        public void Warning(string message)
        {
            if (!IsEnabled(LogLevel.Warning))
            {
                return;
            }

            _logger.LogWarning(message);
        }

        public void Error(string message)
        {
            if (!IsEnabled(LogLevel.Error))
            {
                return;
            }

            _logger.LogError(message);
        }

        public void Debug([InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogDebug(handler.ToStringAndClear());
        }

        public void Information([InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogInformation(handler.ToStringAndClear());
        }

        public void Warning([InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogWarning(handler.ToStringAndClear());
        }

        public void Error([InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogError(handler.ToStringAndClear());
        }

        public void Debug(Exception exception, [InterpolatedStringHandlerArgument("")] ref DebugLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogDebug(exception, handler.ToStringAndClear());
        }

        public void Information(Exception exception, [InterpolatedStringHandlerArgument("")] ref InformationLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogInformation(exception, handler.ToStringAndClear());
        }

        public void Warning(Exception exception, [InterpolatedStringHandlerArgument("")] ref WarningLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogWarning(exception, handler.ToStringAndClear());
        }

        public void Error(Exception exception, [InterpolatedStringHandlerArgument("")] ref ErrorLogInterpolatedStringHandler handler)
        {
            if (!handler.Enabled)
            {
                return;
            }

            _logger.LogError(exception, handler.ToStringAndClear());
        }
    }
}
