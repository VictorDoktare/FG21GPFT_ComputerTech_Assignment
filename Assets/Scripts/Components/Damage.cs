using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Damage : IComponentData
    {
        public int Value;
    }
}
