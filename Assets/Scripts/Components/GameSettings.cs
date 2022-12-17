using Unity.Entities;

namespace Components
{
    public struct GameSettings : IComponentData
    {
        public float LevelWidth;
        public float LevelHeight;
        public int EnemiesToSpawn;
        public float SpawnTimer;
        public float SpawnCountLimit;
    }
}
