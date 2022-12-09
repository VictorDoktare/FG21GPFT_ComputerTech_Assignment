using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct ProjectilePrefab : IComponentData
    {
        public Entity Value;
    }
}
