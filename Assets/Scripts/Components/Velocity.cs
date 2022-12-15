using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct Velocity : IComponentData
    {
        public int Speed;
        public Vector2 Direction;
    }
}
