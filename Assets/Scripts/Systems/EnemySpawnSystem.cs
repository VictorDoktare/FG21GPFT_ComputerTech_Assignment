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
        private float _startTime = 2f;
        private float _spawnTimer = 2f;
        
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

            //Faster spawn the longer game proceeds
            if (_startTime >= 0.2)
            {
                _startTime -= deltaTime * 0.01f;
            }
            
            _spawnTimer -= deltaTime * 1f;
            if (_spawnTimer <= 0)
            {
                setPos.Value.x -= xPosition[Random.Range(0, 5)];
                
                Entities
                    .WithAll<EnemySpawnerTag>()
                    .ForEach((int entityInQueryIndex, in Translation translation) =>
                    {
                        var spawnedEnemyEntity = commandBuffer.Instantiate(entityInQueryIndex, enemy);
                        commandBuffer.SetComponent(entityInQueryIndex, spawnedEnemyEntity, setPos);

                    }).ScheduleParallel();

                _spawnTimer = _startTime;
                _beginSimECB.AddJobHandleForProducer(Dependency);
            }
        }
    }
}
