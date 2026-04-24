namespace SpaceBattle.lib;

public class AuthorizedFireCommand(
    ICommand fireCommand,
    IFireAuthorizer authorizer,
    IShip ship,
    string? accessToken
) : ICommand
{
    public void Execute()
    {
        if (!authorizer.IsAllowed(ship, accessToken))
        {
            return;
        }

        fireCommand.Execute();
    }
}
