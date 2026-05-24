using Microsoft.Extensions.Logging;

namespace Nk7.Logger
{
    public interface ILogLevelLoggerService
    {
        bool IsEnabled(LogLevel logLevel);
    }
}
