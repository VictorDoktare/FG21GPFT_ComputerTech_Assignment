using Components;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct TestOperation
{
    private readonly float _deltaTime;
    private readonly float3 _mousePosition;
    public TestOperation(float deltaTime, float3 mousePosition)
    {
        _deltaTime = deltaTime;
        _mousePosition = mousePosition;
    }
    public float2 SetVelocity(in PlayerInput playerInput, ref Velocity velocity)
    {
        velocity.Direction.xy = playerInput.MoveInput;
        var normalizedDirection = new Vector2(velocity.Direction.x, velocity.Direction.y).normalized;
        return normalizedDirection * velocity.Speed * _deltaTime;
    }
    public float3 SetMouseToWorld()
    {
        if (Camera.main == null)
            return default;

        return Camera.main.ScreenToWorldPoint(_mousePosition);;
    }
    public Quaternion SetRotation(float3 value, ref Translation translation)
    {
        var direction = value - translation.Value;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                
        return Quaternion.AngleAxis(-angle, Vector3.forward);
    }
}
