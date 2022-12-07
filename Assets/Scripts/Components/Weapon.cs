using Unity.Entities;

[GenerateAuthoringComponent]
public struct Weapon : IComponentData
{
    public Entity Projectile;
    public int ProjectileAmount;
    public int ProjectileSpread;
}
