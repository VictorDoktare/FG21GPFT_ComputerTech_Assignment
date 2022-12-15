using Unity.Entities;

namespace Components
{
    public struct GameSettings : IComponentData
    {
        public float LevelWidth;
        public float LevelHeight;
        public int EnemiesToSpawn;
    }
}
