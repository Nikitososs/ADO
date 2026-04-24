namespace SpaceBattle.lib;

public class StoreTorpedoFireRule : IFireRule
{
    public void Apply(FireContext context)
    {
        if (context.Projectile is null || string.IsNullOrWhiteSpace(context.ProjectileId))
        {
            throw new InvalidOperationException("Projectile has not been created.");
        }

        context.GameObjectRepository.Add(context.ProjectileId, context.Projectile);
    }
}
