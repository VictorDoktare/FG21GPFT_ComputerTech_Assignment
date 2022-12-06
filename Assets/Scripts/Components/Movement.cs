using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Movement : IComponentData
{
    [Range(1, 5)] public int Speed;
    public Vector2 Direction;
}
