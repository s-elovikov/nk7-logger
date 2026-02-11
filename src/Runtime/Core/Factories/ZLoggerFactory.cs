using Microsoft.Extensions.Logging;
using System.IO;
using ZLogger;

namespace Nk7.Logger
{
    public static class ZLoggerFactory
    {
        private static readonly UnityDebugFormatter _formatter;

        static ZLoggerFactory()
        {
            _formatter = new UnityDebugFormatter();
        }

        public static ILogger GetLogger(string loggerFilePath = null,
            string categoryName = "Application", LogLevel minimumLogLevel = LogLevel.Error)
        {
            return GetFactory(loggerFilePath, minimumLogLevel)
                .CreateLogger(categoryName);
        }

        private static ILoggerFactory GetFactory(string loggerFilePath, LogLevel minimunLogLevel = LogLevel.None)
        {
            return LoggerFactory.Create(builder =>
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