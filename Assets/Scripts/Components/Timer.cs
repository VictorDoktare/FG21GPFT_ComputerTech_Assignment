using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Timer : IComponentData
    {
        public float Value;
    }
}
