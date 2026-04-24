namespace SpaceBattle.lib;

public class FixedTokenFireAuthorizer(string expectedToken) : IFireAuthorizer
{
    public bool IsAllowed(IShip ship, string? accessToken) =>
        accessToken == expectedToken;
}
