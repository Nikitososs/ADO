
namespace SpaceBattle.lib;

public interface IExceptionHandler
{
    void Handle(ICommand failedCommand, Exception exception);
}
