
namespace SpaceBattle.lib;

public class ExceptionHandlerCommand(ICommand failedCommand, Exception exception) : ICommand
{
    public ICommand FailedCommand { get; } = failedCommand;

    public Exception Exception { get; } = exception;

    public void Execute()
    {
    }
}
