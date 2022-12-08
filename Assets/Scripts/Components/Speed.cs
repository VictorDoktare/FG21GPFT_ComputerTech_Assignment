using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Speed : IComponentData
    {
        public float Value;
    }
}
