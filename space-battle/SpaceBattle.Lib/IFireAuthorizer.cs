namespace SpaceBattle.lib;

public interface IFireAuthorizer
{
    bool IsAllowed(IShip ship, string? accessToken);
}
