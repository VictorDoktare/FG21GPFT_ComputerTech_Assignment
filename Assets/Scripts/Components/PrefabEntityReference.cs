using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct PrefabEntityReference : IComponentData
    {
        public Entity Ref;
    }
}
