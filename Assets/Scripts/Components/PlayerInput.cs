using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct PlayerInput : IComponentData
    {
        public float2 MoveInput;
        public bool FireInput;
    }
}
