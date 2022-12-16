using Unity.Entities;

namespace Components
{
    public struct Weapon : IComponentData
    {
        public int FireRate;
        public float FireDelay;
    }
}
