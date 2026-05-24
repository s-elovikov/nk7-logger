namespace Nk7.Logger
{
    public interface ILoggerService : ILogLevelLoggerService,
        IMessageLoggerService,
        IExceptionLoggerService,
        IInterpolatedStringLoggerService
    {
    }
}
