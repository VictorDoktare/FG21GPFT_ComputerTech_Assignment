using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Direction : IComponentData
    {
        public float2 Value;
    }
}
