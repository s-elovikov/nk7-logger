using System;

namespace Nk7.Logger
{
    public interface IExceptionLoggerService
    {
        void Trace(Exception exception);
        void Debug(Exception exception);
        void Information(Exception exception);
        void Warning(Exception exception);
        void Error(Exception exception);
        void Critical(Exception exception);

        void Trace(Exception exception, string message);
        void Debug(Exception exception, string message);
        void Information(Exception exception, string message);
        void Warning(Exception exception, string message);
        void Error(Exception exception, string message);
        void Critical(Exception exception, string message);
    }
}
