using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    
    public partial class EnemyMovementSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimECB;
        
        protected override void OnCreate()
        {
            _endSimECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var commandBuffer = _endSimECB.CreateCommandBuffer().AsParallelWriter();
            var deltaTime = Time.DeltaTime;
            var time = UnityEngine.Time.time;
        
            Entities
                .WithNone<PlayerTag>()
                //.WithAll<EnemyTag>()
                .ForEach((ref Direction direction, ref Translation translation, in Speed speed,
                    in MovePattern movePattern, in Entity entity) =>
                {
                    //Movement patterns & Velocity
                    if (movePattern.Pattern == MovePattern.Patterns.Wave)
                    {
                        direction.Value.x = Mathf.Cos(time * (speed.Value * movePattern.Frequency));
                        translation.Value.x += (direction.Value.x * speed.Value * deltaTime);
                    }
                    
                    translation.Value.y += direction.Value.y * speed.Value * deltaTime;
                    
                    //Lazy way to destroy enemy when outside of bounds
                    if (translation.Value.y <= -1)
                    {
                        commandBuffer.DestroyEntity(0, entity);
                        return;
                    }

                }).ScheduleParallel();
            
            _endSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}
