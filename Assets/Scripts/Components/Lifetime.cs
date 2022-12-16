using Unity.Entities;

namespace Components
{
    public struct Lifetime : IComponentData
    {
        public float age; 
        public float maxAge;
    }
}
