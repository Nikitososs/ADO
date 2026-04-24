namespace SpaceBattle.lib;

public class CreateTorpedoFireRule : IFireRule
{
    private readonly IIdGenerator _idGenerator;

    public CreateTorpedoFireRule(IIdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public void Apply(FireContext context)
    {
        context.Projectile = context.TorpedoFactory.Create(context.Ship);
        context.ProjectileId = _idGenerator.Generate();
    }
}
