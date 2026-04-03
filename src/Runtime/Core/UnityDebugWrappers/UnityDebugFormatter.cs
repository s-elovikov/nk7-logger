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

            span[0] = (byte)'[';
            span[1] = (byte)(level switch
            {
                LogLevel.Critical => CRITICAL_PREFIX,
                LogLevel.Error => ERROR_PREFIX,
                LogLevel.Warning => WARNING_PREFIX,
                LogLevel.Information => INFORMATION_PREFIX,
                LogLevel.Debug => DEBUG_PREFIX,
                LogLevel.Trace => TRACE_PREFIX,
                _ => UNKNOWN_PREFIX
            });
            span[2] = (byte)']';
            span[3] = (byte)' ';

            writer.Advance(4);
        }
    }
}
