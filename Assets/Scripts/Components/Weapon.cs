using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Weapon : IComponentData
    {
        public int FireRate;
        public int ProjectileAmount;
    }
}
