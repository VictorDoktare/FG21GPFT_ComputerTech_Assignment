using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct PrefabPlayer : IComponentData
    {
        public Entity Reference;
    }
}
