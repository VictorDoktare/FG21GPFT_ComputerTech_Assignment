using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Velocity : IComponentData
{
    [Range(1, 5)] public int Speed;
    public float3 Direction;
    public VelocityPatterns Pattern;
    
    public enum VelocityPatterns
    {
        None,
        Wave
    };
}
