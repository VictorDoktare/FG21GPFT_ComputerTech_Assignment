using Components;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public readonly struct Operation
{
    private readonly float _deltaTime;
    public Operation(float deltaTime)
    {
        _deltaTime = deltaTime;
    }
    
    public float2 SetVelocity(ref Velocity velocity)
    {
        var normalizedDirection = new Vector2(velocity.Direction.x, velocity.Direction.y).normalized;
        return normalizedDirection * velocity.Speed * _deltaTime;
    }
    
    public Quaternion RotateTowards(float3 positionA, float3 positionB)
    {
        var direction = positionA - positionB;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                
        return Quaternion.AngleAxis(-angle, Vector3.forward);
    }
    
    public float3 ScreenToWorldPos(float3 position)
    {
        if (Camera.main == null)
            return default;

        return Camera.main.ScreenToWorldPoint(position);;
    }

    public float CountTime(ref Timer timer, float rate)
    {
        return timer.Value -= _deltaTime * rate;
    }
}
