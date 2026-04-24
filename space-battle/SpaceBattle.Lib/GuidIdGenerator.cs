namespace SpaceBattle.lib;

public class GuidIdGenerator : IIdGenerator
{
    public string Generate()
    {
        return Guid.NewGuid().ToString("N");
    }
}
