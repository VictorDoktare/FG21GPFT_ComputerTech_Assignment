using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Velocity : IComponentData
    {
        public float Speed;
        public float Direction;
    }
}
