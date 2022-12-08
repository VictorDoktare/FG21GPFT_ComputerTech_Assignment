using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct MovePattern : IComponentData
    {
        public Patterns Pattern;
        public int Frequency;
        public enum Patterns
        {
            None,
            Wave
        }
    }
}
