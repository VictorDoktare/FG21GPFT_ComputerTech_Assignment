using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Boundary : IComponentData
    {
        public float2 Value;
    }
}
