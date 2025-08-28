using UnityEngine;

namespace baodeag
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;
        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }
        protected override void Update()
        {
            base.Update();

            //if we dont own this player, dont do anything
            if (!IsOwner)
                return;

            playerLocomotionManager.HandleAllMovement();
        }
    }
}
