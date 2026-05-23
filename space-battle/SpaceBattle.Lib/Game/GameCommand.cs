
using App;

namespace SpaceBattle.lib;

public class GameCommand(object scope) : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", scope).Execute();
        Ioc.Resolve<ICommand>("Game.Scheduler.BeginQuantum").Execute();

        while (Ioc.Resolve<bool>("Game.Scheduler.CanContinue"))
        {
            var cmd = Ioc.Resolve<ICommand>("Game.Scheduler.Take");

            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                Ioc.Resolve<IExceptionHandler>("ExceptionHandler.Handler").Handle(cmd, e);
            }

            Ioc.Resolve<ICommand>("Game.Scheduler.CurrentDate.Advance").Execute();
        }
    }
}
