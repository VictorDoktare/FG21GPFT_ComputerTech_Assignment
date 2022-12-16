using Unity.Entities;
using UnityEngine;

namespace Components.Authoring
{
    public class VelocityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Range(0, 10)][SerializeField] private int speed = 1;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var velocity = default(Velocity);

            velocity.Speed = speed;

            dstManager.AddComponentData(entity, velocity);
        }
    }
}
