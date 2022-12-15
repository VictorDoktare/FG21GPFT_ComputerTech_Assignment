using Unity.Entities;
using UnityEngine;

namespace Components.Authoring
{
    public class PlayerInputAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var playerInput = default(PlayerInput);

            if (Camera.main == null)
                return;

            playerInput.MoveInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            playerInput.MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            playerInput.FireInput = Input.GetButton("Shoot");

            dstManager.AddComponentData(entity, playerInput);
        }
    }
}
