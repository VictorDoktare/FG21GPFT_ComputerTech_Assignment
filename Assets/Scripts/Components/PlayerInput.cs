using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct PlayerInput : IComponentData
    {
        public Vector2 MouseInput;
        public Vector2 MoveInput;
        public bool FireInput;
    }
}
