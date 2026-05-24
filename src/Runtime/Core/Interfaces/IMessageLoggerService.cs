namespace Nk7.Logger
{
    public interface IMessageLoggerService
    {
        void Trace(string message);
        void Debug(string message);
        void Information(string message);
        void Warning(string message);
        void Error(string message);
        void Critical(string message);
    }
}
