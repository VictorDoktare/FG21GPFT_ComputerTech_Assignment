using Components;
using Unity.Entities;

namespace Systems
{
    public partial class HealthSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimECB;
    
        protected override void OnCreate()
        {
            _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }
    
        protected override void OnUpdate()
        {
            var commandBuffer = _beginSimECB.CreateCommandBuffer().AsParallelWriter();

            Entities
                .ForEach((
                    Entity entity,
                    int entityInQueryIndex,
                    ref Health health) => {
                
                    if (health.Value <= 0) 
                    { 
                        commandBuffer.DestroyEntity(entityInQueryIndex, entity); 
                    }
                
                }).Schedule();
        
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}
