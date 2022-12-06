using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerInput : IComponentData
{
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode ShootKey;
}
