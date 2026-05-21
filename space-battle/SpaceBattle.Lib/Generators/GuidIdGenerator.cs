
namespace SpaceBattle.lib;

public class GuidIdGenerator : IIdGenerator
{
    public string NewId() => Guid.NewGuid().ToString();
}
