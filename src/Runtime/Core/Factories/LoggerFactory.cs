using Microsoft.Extensions.Logging;
using System.IO;
using ZLogger;

namespace Nk7.Logger
{
    public static class LoggerFactory
    {
        private static readonly UnityDebugFormatter _formatter;

        static LoggerFactory()
        {
            _formatter = new UnityDebugFormatter();
        }

        public static ILogger GetLogger(LogLevel minimumLogLevel = LogLevel.Error, string loggerFilePath = null)
        {
            return GetFactory(loggerFilePath, minimumLogLevel)
                .CreateLogger("Application");
        }

        private static ILoggerFactory GetFactory(string loggerFilePath, LogLevel minimunLogLevel = LogLevel.None)
        {
            return Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(minimunLogLevel != LogLevel.None
                    ? minimunLogLevel
                    : LogLevel.Warning);

                builder.AddZLoggerStream(new UnityDebugStream(), SetZLoggerStreamOptions);

                if (!string.IsNullOrEmpty(loggerFilePath))
                {
                    string directoryName = Path.GetDirectoryName(loggerFilePath);

                    if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    builder.AddZLoggerFile(loggerFilePath, SetZLoggerFileOptions);
                }
            });
        }

        private static void SetZLoggerStreamOptions(ZLoggerOptions options)
        {
            options.UseFormatter(GetUnityDebugFormatter);
        }

        private static void SetZLoggerFileOptions(ZLoggerOptions options)
        {
            options.UsePlainTextFormatter();
        }

        private static IZLoggerFormatter GetUnityDebugFormatter()
        {
            return _formatter;
        }
    }
}
