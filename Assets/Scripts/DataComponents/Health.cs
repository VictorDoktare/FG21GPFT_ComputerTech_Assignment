using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Health : IComponentData
{
    [Range(0, 10)] public int Value;
}
