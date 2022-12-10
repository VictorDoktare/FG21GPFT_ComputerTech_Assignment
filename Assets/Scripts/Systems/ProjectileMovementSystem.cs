using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial class ProjectileMovementSystem : SystemBase
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
        
            Entities
               .WithAll<ProjectileTag>()
               .ForEach((ref Translation translation, in Direction direction, in Speed speed,
                   in Entity entity) =>
               {
                   //Velocity
                   translation.Value.y += direction.Value.y +1 * speed.Value * deltaTime;

                   //Lazy way to destroy projectile when outside of bounds
                   if (translation.Value.y >= 10)
                   {
                       commandBuffer.DestroyEntity(1, entity);
                   }
                   
               }).ScheduleParallel();
            
            _endSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}
