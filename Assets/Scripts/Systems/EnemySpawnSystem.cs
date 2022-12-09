using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace Systems
{
    public partial class EnemySpawnSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimECB;
        private Entity _enemy;
        private float _timer = 1;
        
        protected override void OnCreate()
        {
            _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }
        
        
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            if (_enemy.Equals(Entity.Null))
            {
                _enemy = GetSingleton<EnemyPrefab>().Value;
                return;
            }
            
            var commandBuffer = _beginSimECB.CreateCommandBuffer().AsParallelWriter();
            var enemy = _enemy;
            var xPosition = new float[]{-2, -1, 0, 1, 2};
            var setPos = new Translation() { Value = new float3(0, 10, 0)};

            _timer -= deltaTime * 1;
            if (_timer <= 0)
            {
                setPos.Value.x -= xPosition[Random.Range(0, 5)];
                
                Entities
                    .WithAll<EnemySpawnerTag>()
                    .ForEach((int entityInQueryIndex, in Translation translation) =>
                    {
                        var spawnedEnemyEntity = commandBuffer.Instantiate(entityInQueryIndex, enemy);
                        commandBuffer.SetComponent(entityInQueryIndex, spawnedEnemyEntity, setPos);

                    }).ScheduleParallel();

                _timer = 1;
                _beginSimECB.AddJobHandleForProducer(Dependency);
            }
        }
    }
}
