using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct PrefabProjectile : IComponentData
    {
        public Entity Reference;
    }
}
