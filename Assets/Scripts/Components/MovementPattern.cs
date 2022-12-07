using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MovementPattern : IComponentData
{
    public enum Patterns
    {
        Straight,
        Wave
    };

    public Patterns patterns;
}
