using UnityEngine;
using Unity.Netcode;

namespace baodeag
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        float vertical;
        float horizontal;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false, 
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            //can be used to stop char from attempting other actions
            //for example, if you get damaged, and begin performing the damage animation
            //the flag will turn true if you are stunned
            //we can then check for this before attempting other actions
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            //tell the server/host we played an animation, and to play that animation on all clients
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId, 
                targetAnimation, 
                applyRootMotion);
        }
    }
}
