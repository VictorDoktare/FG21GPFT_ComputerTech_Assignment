using Unity.Entities;
using UnityEngine;

namespace Components.Authoring
{
    public class PlayerInputAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var playerInput = default(PlayerInput);
            
            dstManager.AddComponentData(entity, playerInput);
        }
    }
}
