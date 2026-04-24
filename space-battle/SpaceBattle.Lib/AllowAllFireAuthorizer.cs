namespace SpaceBattle.lib;

public class AllowAllFireAuthorizer : IFireAuthorizer
{
    public bool IsAllowed(IShip ship, string? accessToken) => true;
}
