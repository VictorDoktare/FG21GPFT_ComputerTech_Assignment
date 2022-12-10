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
            var setPos = new Translation() { Value = new float3(0, 10, 0)};

            //Faster spawn the longer game proceeds
            if (_startTime >= 0.2)
            {
                _startTime -= deltaTime * 0.01f;
            }
            
            _spawnTimer -= deltaTime * 1f;
            
            //var randomPos = Random.Range(-2, 3);
            //var randomSpawnAmount = Random.Range(1, 5);
            
            //setPos.Value.x -= randomPos;
            
            if (_spawnTimer <= 0)
            {
                var randomSpawnAmount = Random.Range(1, 5);

                for (int i = 0; i < randomSpawnAmount; i++)
                {
                    var randomPos = Random.Range(-2, 2);
                    
                    Entities
                        .WithAll<EnemySpawnerTag>()
                        .ForEach((int entityInQueryIndex, in Translation translation) =>
                        {
                            setPos.Value.x = randomPos;
                            var spawnedEnemyEntity = commandBuffer.Instantiate(entityInQueryIndex, enemy);
                            commandBuffer.SetComponent(entityInQueryIndex, spawnedEnemyEntity, setPos);
                            
                        }).ScheduleParallel();
   
                }
                _spawnTimer = _startTime;
                _beginSimECB.AddJobHandleForProducer(Dependency);
            }
        }
    }
}
