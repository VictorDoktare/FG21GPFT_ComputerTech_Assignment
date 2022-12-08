using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Position : IComponentData
    {
        public float2 Value;
    }
}
