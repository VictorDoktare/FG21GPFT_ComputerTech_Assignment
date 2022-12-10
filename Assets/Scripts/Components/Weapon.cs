using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Weapon : IComponentData
    {
        public float3 ProjectileSpawnPos;
        public int NumberOfProjectiles;
        public float ProjectileSpreadRadius;
        public float FireRate;
    }
}
