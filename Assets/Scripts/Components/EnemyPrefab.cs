using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct EnemyPrefab : IComponentData
    {
        public Entity Value;
    }
}
