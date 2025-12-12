using UnityEngine;

namespace baodeag
{
    [CreateAssetMenu(menuName = "Items/Spells/Text Spell")]
    public class TextSpell : SpellItem
    {
        public override void AttemptToCastSpell(PlayerManager player)
        {
            base.AttemptToCastSpell(player);

            if (!CanICastThisSpell(player))
                return;

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(mainHandSpellAnimation, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(offHandSpellAnimation, true);
            }
        }

        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);

            Debug.Log("Casted Spell");
        }

        public override void InstantiateWarmUpSpellFX(PlayerManager player)
        {
            base.InstantiateWarmUpSpellFX(player);

            Debug.Log("Instantiated FX");
        }

        public override void InstantiateReleaseSpellFX(PlayerManager player)
        {
            base.InstantiateReleaseSpellFX(player);

            Debug.Log("Instantiated Projectile");
        }

        public override bool CanICastThisSpell(PlayerManager player)
        {
            if (player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isJumping.Value)
                return false;

            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return false;


            return true;
        }
    }
}
