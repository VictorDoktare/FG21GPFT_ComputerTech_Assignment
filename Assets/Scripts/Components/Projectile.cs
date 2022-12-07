using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct Projectile : IComponentData
{
    public int Damage;
}
