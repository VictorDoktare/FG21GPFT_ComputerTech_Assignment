using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct PrefabEnemy : IComponentData
    {
        public Entity Reference;
    }
}
