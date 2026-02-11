using Microsoft.Extensions.Logging;
using ZLogger.Formatters;
using System.Buffers;
using ZLogger;

namespace Nk7.Logger
{
    internal sealed class UnityDebugFormatter : IZLoggerFormatter
    {
        public const char CRITICAL_PREFIX = 'C';
        public const char ERROR_PREFIX = 'E';
        public const char WARNING_PREFIX = 'W';
        public const char INFORMATION_PREFIX = 'I';
        public const char DEBUG_PREFIX = 'D';
        public const char TRACE_PREFIX = 'T';
        public const char UNKNOWN_PREFIX = '?';

        public const string PART_OF_PREFIX = "[{0}]";

        private readonly PlainTextZLoggerFormatter _inner = new PlainTextZLoggerFormatter();

        public bool WithLineBreak => _inner.WithLineBreak;

        public void FormatLogEntry(IBufferWriter<byte> writer, IZLoggerEntry entry)
        {
            var level = LogLevel.None;

            if (entry is INonReturnableZLoggerEntry logEntry)
            {
                level = logEntry.LogInfo.LogLevel;
            }

            WritePrefix(writer, level);

            _inner.FormatLogEntry(writer, entry);
        }

        private static void WritePrefix(IBufferWriter<byte> writer, LogLevel level)
        {
            var span = writer.GetSpan(4);
            string prefix = GetLevelPrefix(level);

            span[0] = (byte)prefix[0];
            span[1] = (byte)prefix[1];
            span[2] = (byte)prefix[2];
            span[3] = (byte)' ';

            writer.Advance(4);
        }

        private static string GetLevelPrefix(LogLevel level)
        {
            return string.Format(PART_OF_PREFIX, level switch
            {
                LogLevel.Critical => CRITICAL_PREFIX,
                LogLevel.Error => ERROR_PREFIX,
                LogLevel.Warning => WARNING_PREFIX,
                LogLevel.Information => INFORMATION_PREFIX,
                LogLevel.Debug => DEBUG_PREFIX,
                LogLevel.Trace => TRACE_PREFIX,
                _ => UNKNOWN_PREFIX
            });
        }
    }
}
