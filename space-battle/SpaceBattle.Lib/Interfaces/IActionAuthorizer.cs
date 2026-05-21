
namespace SpaceBattle.lib;

public interface IActionAuthorizer
{
    bool CanPerform(string userId, string objectId, string action);
}
